using declared_persons_analyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace declared_persons_analyser.Services
{
    public class DataServices
    {

        public List<Value> FilterData(Options userData, List<Value> data)
        {
            List<Value> filteredData = data;

            filteredData = FilterByYear(userData, filteredData);
            filteredData = FilterByMonth(userData, filteredData);
            filteredData = FilterByDay(userData, filteredData);
            filteredData = FilterByGroup(userData, filteredData);
            filteredData = FilterByLimit(userData, filteredData);

            return filteredData;
        }

        private List<Value> FilterByYear(Options userData, List<Value> data)
        {
            if (userData.Year == 0) return data;

            return data.Where(x => x.year == userData.Year).ToList();
        }

        private List<Value> FilterByMonth(Options userData, List<Value> data)
        {
            if (userData.Month == 0) return data;

            return data.Where(x => x.month == userData.Month).ToList();
        }

        private List<Value> FilterByDay(Options userData, List<Value> data)
        {
            if (userData.Day == 0) return data;

            return data.Where(x => x.day == userData.Day).ToList();
        }

        private List<Value> FilterByLimit(Options userData, List<Value> data)
        {
            return data.Take(userData.Limit).ToList();
        }

        private List<Value> FilterByGroup(Options userData, List<Value> data)
        {
            if (String.IsNullOrEmpty(userData.Group)) return data;
            var districtName = data.First().district_name;

            switch (userData.Group.Trim().ToLower())
            {
                case "y":

                    return data.GroupBy(x => x.year).OrderBy(x => x.Key).Select(x => new Value { district_id = userData.District, district_name = districtName, value = x.Sum(x => x.value), year = x.Key }).ToList();

                case "m":

                    return data.GroupBy(x => x.month).OrderBy(x => x.Key).Select(x => new Value { district_id = userData.District, district_name = districtName, value = x.Sum(x => x.value), month = x.Key }).ToList();

                case "d":

                    return data.GroupBy(x => x.day).OrderBy(x => x.Key).Select(x => new Value { district_id = userData.District, district_name = districtName, value = x.Sum(x => x.value), day = x.Key }).ToList();

                case "ym":

                    return data.GroupBy(x => new { x.year, x.month }).OrderBy(x => x.Key.year).ThenBy(x => x.Key.month).Select(x => new Value { district_id = userData.District, district_name = districtName, value = x.Sum(x => x.value), year = x.Key.year, month = x.Key.month }).ToList();

                case "yd":

                    return data.GroupBy(x => new { x.year, x.day }).OrderBy(x => x.Key.year).ThenBy(x => x.Key.day).Select(x => new Value { district_id = userData.District, district_name = districtName, value = x.Sum(x => x.value), year = x.Key.year, day = x.Key.day }).ToList();

                case "md":

                    return data.GroupBy(x => new { x.month, x.day }).OrderBy(x => x.Key.month).ThenBy(x => x.Key.day).Select(x => new Value { district_id = userData.District, district_name = districtName, value = x.Sum(x => x.value), day = x.Key.day, month = x.Key.month }).ToList();

                default: Console.WriteLine("Nepareizs -group parametrs, lietojiet vienu no sekojošajiem: y, m, d, ym, yd, md \n"); throw new InvalidOperationException();

            }
        }

    }
}
