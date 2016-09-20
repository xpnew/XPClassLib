using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util.Bytes
{
   public  class Int32Byte
    {
       public byte High { get; set; }


       public byte Low { get; set; }



       public byte[] Output { get; set; }



       public Int32Byte():this(0)
       {
       }

       public int Source { get; set; }

       public Int32Byte(int input)
       {
           Source = input;

           Output = new byte[4];
           _Analyze();
       }

       private void _Analyze()
       {
           if (0 == Source)
               return;


           Low = (byte)(Source % 256);
           High = (byte)(Source / 256);

           Output = BitConverter.GetBytes(Source);

       }
       public static Int32Byte Analyze(int source)
       {
           Int32Byte tool = new Int32Byte(source);
           return tool;           
       }
       

 

    }
}
