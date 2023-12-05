namespace AdventOfCode2023
{
    public static class Day5
    {
        public static async Task<long> One()
        {
            var data = await GetDayOneData();
            var parsedData = ParseData(data);
            long? veryLowestNumber = null;
            foreach (var seed in parsedData.Seeds)
            {
                long lowestLocationNumber = GetLowestLocationNumber(seed, parsedData);
                if (veryLowestNumber is null || lowestLocationNumber < veryLowestNumber)
                {
                    veryLowestNumber = lowestLocationNumber;
                }
            }
            return veryLowestNumber ?? 0;
        }

        private static long GetLowestLocationNumber(long seed, Maps parsedData)
        {
            long soilNumber = GetMapped(seed, parsedData.SeedToSoil);
            long fertNumber = GetMapped(soilNumber, parsedData.SoilToFertilizer);
            long waterNumber = GetMapped(fertNumber, parsedData.FertilizerToWater);
            long lightNumber = GetMapped(waterNumber, parsedData.WaterToLight);
            long tempNumber = GetMapped(lightNumber, parsedData.LightToTemp);
            long humNumber = GetMapped(tempNumber, parsedData.TempToHumidity);
            long locNumber = GetMapped(humNumber, parsedData.HumidityToLocation);

            return locNumber;
        }

        private static long GetMapped(long source, List<TRange> ranges)
        {
            long? dest = null;
            foreach (var range in ranges)
            {
                if (source < range.SourceRangeStart || range.SourceRangeStart + range.RangeLength < source)
                {
                    continue;
                }
                var diff = source - range.SourceRangeStart;
                dest = range.DestRangeStart + diff;
                break;
            }
            return dest ?? source;
        }

        public static async Task<long> Two()
        {
            var data = await GetDayTwoData();
            var parsedData = ParseData(data);
            long? veryLowestNumber = null;
            for (int i = 0; i < parsedData.Seeds.Count; i++)
            {
                for (long j = 0; j < parsedData.Seeds[i + 1]; j++)
                {
                    long lowestLocationNumber = GetLowestLocationNumber(parsedData.Seeds[i] + j, parsedData);
                    if (veryLowestNumber is null || lowestLocationNumber < veryLowestNumber)
                    {
                        veryLowestNumber = lowestLocationNumber;
                    }
                }
                i++;
            }
            return veryLowestNumber ?? 0;
        }

        private static Maps ParseData(string[] data)
        {
            Maps maps = new Maps();
            int mappingIndex = 0;
            maps.Seeds = data[0].Split(": ")[1].Split(" ").Select(numStr => long.Parse(numStr)).ToList();
            List<TRange> itemToPopulate = new();
            foreach (var item in data[1..])
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    if (mappingIndex != 0)
                    {
                        if (mappingIndex == 1)
                        {
                            maps.SeedToSoil = itemToPopulate;
                        }
                        else if (mappingIndex == 2)
                        {
                            maps.SoilToFertilizer = itemToPopulate;
                        }
                        else if (mappingIndex == 3)
                        {
                            maps.FertilizerToWater = itemToPopulate;
                        }
                        else if (mappingIndex == 4)
                        {
                            maps.WaterToLight = itemToPopulate;
                        }
                        else if (mappingIndex == 5)
                        {
                            maps.LightToTemp = itemToPopulate;
                        }
                        else if (mappingIndex == 6)
                        {
                            maps.TempToHumidity = itemToPopulate;
                        }
                        //else if (mappingIndex == 7)
                        //{
                        //    maps.HumidityToLocation = itemToPopulate;
                        //}
                        itemToPopulate = new List<TRange>();
                    }
                    continue;
                }
                else if (item.StartsWith("seed-to-soil"))
                {
                    mappingIndex = 1;
                }
                else if (item.StartsWith("soil-to-fertilizer"))
                {
                    mappingIndex = 2;
                }
                else if (item.StartsWith("fertilizer-to-water"))
                {
                    mappingIndex = 3;
                }
                else if (item.StartsWith("water-to-light"))
                {
                    mappingIndex = 4;
                }
                else if (item.StartsWith("light-to-temperature"))
                {
                    mappingIndex = 5;
                }
                else if (item.StartsWith("temperature-to-humidity"))
                {
                    mappingIndex = 6;
                }
                else if (item.StartsWith("humidity-to-location"))
                {
                    mappingIndex = 7;
                }
                else
                {
                    TRange rangeLine = ParseRangeLine(item);
                    itemToPopulate.Add(rangeLine);
                }
            }
            maps.HumidityToLocation = itemToPopulate;
            return maps;
        }

        private static TRange ParseRangeLine(string item)
        {
            var data = item.Split(" ");
            long destinationRangeStart = long.Parse(data[0]);
            long sourceRangeStart = long.Parse(data[1]);
            long rangeLength = long.Parse(data[2]);

            return new TRange
            {
                DestRangeStart = destinationRangeStart,
                SourceRangeStart = sourceRangeStart,
                RangeLength = rangeLength
            };
        }

        private class Maps
        {
            public List<long> Seeds { get; set; }
            public List<TRange> SeedToSoil { get; set; }
            public List<TRange> SoilToFertilizer { get; set; }
            public List<TRange> FertilizerToWater { get; set; }
            public List<TRange> WaterToLight { get; set; }
            public List<TRange> LightToTemp { get; set; }
            public List<TRange> TempToHumidity { get; set; }
            public List<TRange> HumidityToLocation { get; set; }
        }

        private class TRange
        {
            public long DestRangeStart { get; set; }
            public long SourceRangeStart { get; set; }
            public long RangeLength { get; set; }
        }

        private static async Task<string[]> GetDayOneData()
        {
            return await File.ReadAllLinesAsync("DayFiveOneData.txt");
        }

        private static async Task<string[]> GetDayTwoData()
        {
            return await File.ReadAllLinesAsync("DayFiveTwoData.txt");
        }
    }
}
