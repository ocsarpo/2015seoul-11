using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using System.Threading.Tasks;

namespace App5
{
    class DrawShopMap
    {
        private DrawInfoInMap drawMap;
        public DrawInfoInMap ProPerty_DrawMap { get { return drawMap; } }
        public DrawInfoInMap DrawMap
        {
            get { return drawMap; }
        }

        public DrawShopMap(string localKey, ref Bitmap originImg, ref Bitmap canvas, ref Bitmap indicator)
        {
            drawMap = new DrawInfoInMap(localKey,ref originImg,ref canvas, ref indicator);
        }

        public void DrawPointMap(string text, ScaleImageView image)
        {
            drawMap.DrawPointMap(text, image);
        }
    }

    public class DrawInfoInMap
    {
        Bitmap origin = null;
        Bitmap canvas = null;
        Bitmap indicator = null;
        string localKey;
        int DRAW_KEY;

        public DrawInfoInMap(string localKey, ref Bitmap origin, ref Bitmap canvas, ref Bitmap indicator)
        {
            this.localKey = localKey;
            this.origin = origin;
            this.canvas = canvas;
            this.indicator = indicator;
            DRAW_KEY = 1;
        }

        public void DrawPointMap(string category, ScaleImageView image)
        {
            if (DRAW_KEY == 1)
            {
                DRAW_KEY = 0;
                Canvas c=null;
                Paint p=null;
                try {
                 c = new Canvas(this.canvas);
                c.DrawBitmap(this.origin, 0, 0, null);

                 p = new Paint();
                p.Color = Color.Red;
                }
                catch(System.Exception e)
                { }
                //////////////////////////////////////////

                foreach (var value in Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[localKey])
                {
                    if (category.Equals(value.EngCategory))
                    {
                        c.DrawBitmap(this.indicator, value.ShopLocation.X * MainActivity.DPI_XSCALE
                            , value.ShopLocation.Y * MainActivity.DPI_YSCALE, null);
                        //c.DrawCircle(value.ShopLocation.X * MainActivity.DPI_XSCALE
                        //    , value.ShopLocation.Y * MainActivity.DPI_YSCALE, 12, p);
                 
                    }
                }
                image.SetImageBitmap(this.canvas);

                DRAW_KEY = 1;
            }
        }
    }
}