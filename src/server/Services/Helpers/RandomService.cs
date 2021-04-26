using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Services.Helpers
{
  public class RandomService
  {
    public static int GenerateRandomInt(int a, int b)
    {
      Random r = new();
      int randomInteger = r.Next(a, b);
      return randomInteger;
    }

    public static float GenerateRandomFloat(int addToFloat)
    {
      Random r = new();
      double randomDouble = r.NextDouble() + addToFloat;
      return (float)randomDouble;
    }
  }
}
