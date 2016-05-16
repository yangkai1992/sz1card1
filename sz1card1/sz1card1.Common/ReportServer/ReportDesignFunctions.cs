using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace sz1card1.Common
{
    public class ReportDesignFunctions
    {
        public static string GetProvinceNameByProvinceId(string provinceId)
        {
            string provinceName  = DataUtil.GetProvinceById(provinceId);
            return provinceName;
        }
        public static string GetCityNameByCityId(string cityId)
        {
            string cityName = DataUtil.GetCityById(cityId);
            return cityName;
        }
        public static string GetCountyNameByCountyId(string countyId)
        {
            string countyName = DataUtil.GetCountyById(countyId);
            return countyName;
        }
    }
}
