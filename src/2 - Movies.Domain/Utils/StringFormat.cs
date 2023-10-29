using System.Text;

namespace Movies.Domain.Utils;

public static class StringFormat
{
    public static string ToString(List<string> strList)
        {
            StringBuilder builder = new();
            
            strList.ForEach(str => {
                builder.Append(str);
                builder.Append('\n');
            });

            builder.Remove(builder.Length-1, 1);

            return builder.ToString();
        }
}
