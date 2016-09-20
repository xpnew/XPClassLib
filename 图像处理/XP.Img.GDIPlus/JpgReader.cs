using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace XP.Img.GDIPlus
{
    public class JpgReader
    {



        public Image Pic { get; set; }


        public JpgReader()
        {
            Pic = null;
        }

        public void Load(string path)
        {
            try
            {
                Image img = Image.FromFile(path);
                Pic = img;
            }
            catch (Exception ex)
            {


            }
        }


    }
}
