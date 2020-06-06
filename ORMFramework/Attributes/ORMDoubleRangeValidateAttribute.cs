using System;
using System.Collections.Generic;
using System.Text;

namespace 手写ORM.ORMFramework.Attributes
{
    public class ORMDoubleRangeValidateAttribute:ORMBaseValidateAttribute
    {
  

        private double _dMax =0;
        private double _dMin = 0;

        public ORMDoubleRangeValidateAttribute(double min,double max)
        {
            this._dMax = max;
            this._dMin = min;
        }


        public override bool Validate(object o)
        {
            return o != null &&
                !string.IsNullOrEmpty(o.ToString()) &&
                double.TryParse(o.ToString(), out double r) &&
                double.Parse(o.ToString()) >= _dMin &&
                double.Parse(o.ToString()) < _dMax;
        }

    }
}
