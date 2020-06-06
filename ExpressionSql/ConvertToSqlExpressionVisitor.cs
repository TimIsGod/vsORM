using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionSql
{
    // Expression 的本质是二叉树，所以将Expression转成where字句，就是从叶子往回捋
    public class ConvertToSqlExpressionVisitor : ExpressionVisitor
    {
        // 惯用伎俩 用栈来重新排序
        private Stack<string> ConditionStack = new Stack<string>();

        public String GetWhere()
        {
            string where = string.Concat(this.ConditionStack.ToArray());
            this.ConditionStack.Clear();
            return where;
        }

        /// <summary>
        /// 通过基类的Visit入口将表达式传入，这个方法会先分开左边和右边
        /// 然后会根据节点数据的类型自动查找处理该类型的visit方法进行处理
        /// 然后将叶子节点的内容进行倒序压栈，因为栈的特点，才能正序取出
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override Expression Visit(Expression node)
        {
            /** 传入进来的表达式显示为：
            * {($c.Id > 1L && $c.Id < 10L || $c.Id == 5L) 
            * && .Call($c.Number).StartsWith("1") 
            * && .Call($c.Number).EndsWith("2") 
            * && .Call($c.Number).Contains("3")}
            * Expression 会从最后一个条件进行二叉树的拆分，每个节点都是一个Expression
            * 直到最后的叶子节点，NodeType 为 MemberAccess
            */
            return base.Visit(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            /** 
             * 传入的node的NodeType为逻辑运算符
             * 且将最后一个最高优先级的逻辑预算符两边的内容进行了拆分
             * 利用栈对两边内容进行重新排序
             * 注意：左右两边的内容并没有在这里压栈，只有他们进入到 memberAccess类型处理时才会压栈
             */
            this.ConditionStack.Push(")");
            base.Visit(node.Right);
            this.ConditionStack.Push(node.NodeType.GetSqlOperator());
            base.Visit(node.Left);
            this.ConditionStack.Push("(");
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            //进入到这里的NodeType是Constant，说明已经是常量了
            this.ConditionStack.Push($"'{node.Value.ToString()}'");
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            //进入到这里的NodeType为MemberAccess，说明是一个对象成员的叶子节点
            //叶子节点可以压栈
            this.ConditionStack.Push($"{node.Member.Name}");
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {

            
            if (node == null)
            {
                throw new ArgumentNullException("方法为空！");
            }
            /**
             * 进到这里的节点类型为Call，说明是在呼叫一个方法
             * 对于sql的where字句而言，目前先只处理Like
             * 
             */
            string format;

            switch (node.Method.Name)
            {
                case "StartsWith":
                    format = "({0} LIKE {1}+'%')";
                    break;

                case "Contains":
                    format = "({0} LIKE '%'+{1}+'%')";
                    break;

                case "EndsWith":
                    format = "({0} LIKE '%'+{1})";
                    break;
            
                default:
                    throw new NotSupportedException(node.NodeType + " is not supported!");
            }
            //注意这里没有对list.Contains的操作进行处理，需要继续扩展
            this.Visit(node.Object);
            this.Visit(node.Arguments[0]);
            //这里是把刚刚压入栈的成员和常量弹出来，重新组成like语句
            string right = this.ConditionStack.Pop();
            string left = this.ConditionStack.Pop();
            this.ConditionStack.Push(string.Format(format, left, right));

            return node;

        }
    }
}
