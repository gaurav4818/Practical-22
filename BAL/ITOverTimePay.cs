using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class ITOverTimePay : IOvertimePay
    {
      

        public int MyOverTimePay(int hour)
        {
            int departmentpay = 200;
            return hour * departmentpay;
        }
    }
}
