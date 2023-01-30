namespace Image_processing.Managers
{
    public static class arrayExtension
    {
        public static List<List<T>> ToList<T>(this T[,] array)
        {
            var result = new List<List<T>>();
            var lengthX = array.GetLength(0);
            var lengthY = array.GetLength(1);
            
            for (int i = 0; i < lengthX; i++)
            {
                var listToAdd = new List<T>(lengthY);

                for (int i2 = 0; i2 < lengthY; i2++)
                {
                    listToAdd.Add(array[i, i2]);
                }

                result.Add(listToAdd);
            }

            return result;
        }
    }
}