using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Util;
using System.Collections.Generic;

namespace App5
{
    class ScaleImageViewGestureDetector : GestureDetector.SimpleOnGestureListener
    {

        private readonly ScaleImageView m_ScaleImageView;
        public ScaleImageViewGestureDetector(ScaleImageView imageView)
        {
            m_ScaleImageView = imageView;
        }


        public override bool OnDown(MotionEvent e)
        {
            return true;
        }

        public override bool OnDoubleTap(MotionEvent e)
        {
            m_ScaleImageView.MaxZoomTo((int)e.GetX(), (int)e.GetY());
            m_ScaleImageView.Cutting();
            m_ScaleImageView.DispatchTouchEvent(MotionEvent.Obtain(Android.OS.SystemClock.UptimeMillis(),
                Android.OS.SystemClock.UptimeMillis() + 100, MotionEventActions.Move, 0f, 0f, 0));
            return true;
        }
    }

    public class ScaleImageView : ImageView, View.IOnTouchListener
    {
        private Context m_Context;
        private float m_MaxScale = 3.0f;
        private Matrix m_Matrix;
        private float[] m_MatrixValues = new float[9];
        private int m_Width;
        private int m_Height;
        private int m_IntrinsicWidth;
        private int m_IntrinsicHeight;
        private float m_Scale;
        private float m_MinScale;
        private float m_PreviousDistance;
        private int m_PreviousMoveX;
        private int m_PreviousMoveY;

        private float toX;
        private float toY;
        private bool m_IsScaling;
        private GestureDetector m_GestureDetector;
        List<ShopNameLocation> shopNameList;
        private string localKey;
        public string LocalKey
        { set { localKey = value; } }
        private TextView marker = null;

        public ScaleImageView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            m_Context = context;
            Initialize();
        }

        public ScaleImageView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            m_Context = context;
            Initialize();
        }
        public void SetImageBitmapAndMarker(Bitmap bm, List<ShopNameLocation> shopNameList)
        {
            if (shopNameList != null)
                this.shopNameList = shopNameList;
            SetImageBitmap(bm);
        }
        public void SetImageBitmapAndMarker(Bitmap bm, TextView marker, float toX, float toY, List<ShopNameLocation> shopNameList)
        {
            if (marker != null)
            {
                this.marker = marker;
                this.toX = toX;
                this.toY = toY;
            }
            if (shopNameList != null)
                this.shopNameList = shopNameList;
            SetImageBitmap(bm);
        }

        public override void SetImageBitmap(Bitmap bm)
        {
            base.SetImageBitmap(bm);
            this.Initialize();
        }

        public void SetImageResourceAndMarker(int resId, TextView marker, float toX, float toY, List<ShopNameLocation> shopNameList)
        {
            if (marker != null)
            {
                this.marker = marker;
                this.toX = toX;
                this.toY = toY;
            }
            if (shopNameList != null)
                this.shopNameList = shopNameList;
            SetImageResource(resId);
        }

        public override void SetImageResource(int resId)
        {
            base.SetImageResource(resId);
            this.Initialize();
        }

        private void Initialize()
        {
            this.SetScaleType(ScaleType.Matrix);
            m_Matrix = new Matrix();

            if (Drawable != null)
            {
                m_IntrinsicWidth = Drawable.IntrinsicWidth;
                m_IntrinsicHeight = Drawable.IntrinsicHeight;
                this.SetOnTouchListener(this);
            }


            m_GestureDetector = new GestureDetector(m_Context, new ScaleImageViewGestureDetector(this));
        }


        protected override bool SetFrame(int l, int t, int r, int b)
        {
            m_Width = r - l;
            m_Height = b - t;


            m_Matrix.Reset();
            var r_norm = r - l;
            m_Scale = (float)r_norm / (float)m_IntrinsicWidth;


            var paddingHeight = 0;
            var paddingWidth = 0;
            if (m_Scale * m_IntrinsicHeight > m_Height)
            {
                m_Scale = (float)m_Height / (float)m_IntrinsicHeight;
                m_Matrix.PostScale(m_Scale, m_Scale);
                paddingWidth = (r - m_Width) / 2;
            }
            else
            {
                m_Matrix.PostScale(m_Scale, m_Scale);
                paddingHeight = (b - m_Height) / 2;
            }

            //if (marker != null)
            //{
            //    m_Matrix.PostTranslate(-(toX - (m_Width / 2)) * Scale, -(toY - (m_Height / 2)) * Scale);
            //    marker.TranslationX += (toX - (m_Width / 2)) * Scale;
            //    marker.TranslationY += (toY - (m_Height / 2)) * Scale;
            //    if (shopNameList != null)
            //    {
            //        foreach (var i in this.shopNameList)
            //        {
            //            i.myView.TranslationX += (toX - (m_Width / 2)) * Scale;
            //            i.myView.TranslationY += (toY - (m_Height / 2)) * Scale;
            //        }
            //    }
            //}

            //else
                m_Matrix.PostTranslate(paddingWidth, paddingHeight);

            ImageMatrix = m_Matrix;
            m_MinScale = m_Scale + 0.1f;
            ZoomTo(m_Scale, m_Width / 2, m_Height / 2);
            Cutting();

            MoveMarker();
    //        this.DispatchTouchEvent(MotionEvent.Obtain(Android.OS.SystemClock.UptimeMillis(),
    //Android.OS.SystemClock.UptimeMillis() + 100, MotionEventActions.Move, 0f, 0f, 0));
            return base.SetFrame(l, t, r, b);
        }

        private void MoveMarker()
        {
            if (marker != null)
            {
                marker.TranslationX = (this.toX * MainActivity.DPI_XSCALE * m_MatrixValues[0] - Math.Abs(m_MatrixValues[2]));
                marker.TranslationY = (this.toY * MainActivity.DPI_YSCALE * m_MatrixValues[4] - Math.Abs(m_MatrixValues[5]));
            }
            if (shopNameList != null)
                foreach (var i in this.shopNameList)
                {
                    i.myView.TranslationX = (i.myLocationX * MainActivity.DPI_XSCALE * m_MatrixValues[0] - Math.Abs(m_MatrixValues[2]));
                    i.myView.TranslationY = (i.myLocationY * MainActivity.DPI_YSCALE * m_MatrixValues[4] - Math.Abs(m_MatrixValues[5]));
                }
        }

        private float GetValue(Matrix matrix, int whichValue)
        {
            matrix.GetValues(m_MatrixValues);
            return m_MatrixValues[whichValue];
        }

        public float Scale
        {
            get { return this.GetValue(m_Matrix, Matrix.MscaleX); }
        }


        public float TranslateX
        {
            get { return this.GetValue(m_Matrix, Matrix.MtransX); }
        }


        public float TranslateY
        {
            get { return this.GetValue(m_Matrix, Matrix.MtransY); }
        }


        public void MaxZoomTo(int x, int y)
        {
            if (this.m_MinScale != this.Scale && (Scale - m_MinScale) > 0.1f)
            {
                var scale = m_MinScale / Scale;
                ZoomTo(scale, x, y);
            }
            else
            {
                var scale = m_MaxScale / Scale;
                ZoomTo(scale, x, y);
            }
        }


        public void ZoomTo(float scale, int x, int y)
        {
            if (Scale * scale < m_MinScale)
            {
                scale = m_MinScale / Scale;
            }
            else
            {
                if (scale >= 1 && Scale * scale > m_MaxScale)
                {
                    scale = m_MaxScale / Scale;
                }
            }
            m_Matrix.PostScale(scale, scale);
            //move to center 
            m_Matrix.PostTranslate(-(m_Width * scale - m_Width) / 2, -(m_Height * scale - m_Height) / 2);

            //move x and y distance 
            m_Matrix.PostTranslate(-(x - (m_Width / 2)) * scale, 0);
            m_Matrix.PostTranslate(0, -(y - (m_Height / 2)) * scale);
            ImageMatrix = m_Matrix;

            int quantityControlval = 1;
            int count = 0;
            if (shopNameList != null)
                foreach (var i in shopNameList)
                {
                    if (Scale * scale < 1.5f)
                    {
                        quantityControlval = 3;
                        if (count++ % quantityControlval != 0)
                            i.myView.Visibility = ViewStates.Invisible;
                        else
                            i.myView.Visibility = ViewStates.Visible;
                    }
                    else if (Scale * scale >= 1.5f && Scale * scale < 2f)
                    {
                        quantityControlval = 2;
                        if (count++ % quantityControlval != 0)
                            i.myView.Visibility = ViewStates.Invisible;
                        else
                            i.myView.Visibility = ViewStates.Visible;
                    }

                    else if (Scale * scale >= 2f && Scale * scale < 2.5f)
                    {
                        quantityControlval = 3;
                        if (count++ % quantityControlval == 0)
                            i.myView.Visibility = ViewStates.Invisible;
                        else
                            i.myView.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        i.myView.Visibility = ViewStates.Visible;
                    }

                    if (i.myView.Tag.ToString().Split(',')[1].Equals("REST") ||
                        i.myView.Tag.ToString().Split(',')[1].Equals("GATEWAY") ||
                        i.myView.Tag.ToString().Split(',')[1].Equals("SUBWAY") ||
                        i.myView.Tag.ToString().Split(',')[1].Equals("MOVE"))
                    {
                        i.myView.Visibility = ViewStates.Visible;
                    }
                    if (marker != null)
                    {
                        if (i.myView.Tag.ToString().Split(',')[0].Equals(marker.Tag.ToString()))
                            i.myView.Visibility = ViewStates.Visible;
                        marker.Visibility = ViewStates.Visible;
                    }
                }
            count = 0;
        }

        public void Cutting()
        {
            var width = (int)(m_IntrinsicWidth * Scale);
            var height = (int)(m_IntrinsicHeight * Scale);
            if (TranslateX < -(width - m_Width))
            {
                m_Matrix.PostTranslate(-(TranslateX + width - m_Width), 0);
            }


            if (TranslateX > 0)
            {
                m_Matrix.PostTranslate(-TranslateX, 0);
            }


            if (TranslateY < -(height - m_Height))
            {
                m_Matrix.PostTranslate(0, -(TranslateY + height - m_Height));
            }


            if (TranslateY > 0)
            {
                m_Matrix.PostTranslate(0, -TranslateY);
            }


            if (width < m_Width)
            {
                m_Matrix.PostTranslate((m_Width - width) / 2, 0);
            }


            if (height < m_Height)
            {
                m_Matrix.PostTranslate(0, (m_Height - height) / 2);
            }


            ImageMatrix = m_Matrix;
        }


        private float Distance(float x0, float x1, float y0, float y1)
        {
            var x = x0 - x1;
            var y = y0 - y1;
            return (float)Math.Sqrt(x * x + y * y);
        }

        private float DispDistance()
        {
            return (float)Math.Sqrt(m_Width * m_Width + m_Height * m_Height);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {

            if (m_GestureDetector.OnTouchEvent(e))
            {
                m_PreviousMoveX = (int)e.GetX();
                m_PreviousMoveY = (int)e.GetY();
                return true;
            }

            var touchCount = e.PointerCount;

            //switch (e.Action) //터치 했을때
            //{
            //    case MotionEventActions.Up:
            //        if (Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO.ContainsKey(localKey))
            //        {
            //            MoveMarker();
            //            foreach (ShopInformation shopInfo in Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[localKey])
            //            {
            //                Rect pointRect = new Rect();
            //                pointRect.Left = (int)((shopInfo.ShopLocation.X*500)/720 * MainActivity.DPI_XSCALE) - 10;
            //                pointRect.Top = (int)((shopInfo.ShopLocation.Y * 500) / 1281 * MainActivity.DPI_YSCALE) - 10;
            //                pointRect.Right = (int)((shopInfo.ShopLocation.X * 500) / 720 * MainActivity.DPI_XSCALE) + 10;
            //                pointRect.Bottom = (int)((shopInfo.ShopLocation.Y * 500) / 1281 * MainActivity.DPI_YSCALE) + 10;

            //                int x = (int)(((e.GetX() + Math.Abs(m_MatrixValues[2])) / m_MatrixValues[0])*500/720 );
            //                int y = (int)(((e.GetY() + Math.Abs(m_MatrixValues[5])) / m_MatrixValues[4])*500/1281);
            //                //int x = (int)((e.GetX() / m_MatrixValues[0]) + (Math.Abs(m_MatrixValues[2]) / m_MatrixValues[0]));
            //                //int y = (int)((e.GetY() / m_MatrixValues[4]) + (Math.Abs(m_MatrixValues[5]) / m_MatrixValues[4]));
            //                if (pointRect.Contains(x, y))
            //                {
            //                    var second = new Intent(this.Context, typeof(ShopInfoActivity));
            //                    second.PutExtra("SHOP_ID_NUMBER", shopInfo.IdentificationNumber);
            //                    second.PutExtra("Local", localKey);
            //                    this.Context.StartActivity(second);
            //                    break;
            //                }
            //            }
            //        }
            //        break;
            //}


            switch (e.Action) //확대, 축소
            {
                case MotionEventActions.Down:


                case MotionEventActions.Pointer1Down:
                case MotionEventActions.Pointer2Down:
                    {
                        if (touchCount >= 2)
                        {
                            var distance = this.Distance(e.GetX(0), e.GetX(1), e.GetY(0), e.GetY(1));
                            m_PreviousDistance = distance;
                            m_IsScaling = true;
                        }
                    }
                    break;

                case MotionEventActions.Move:
                    {
                        if (touchCount >= 2 && m_IsScaling)
                        {
                            var distance = this.Distance(e.GetX(0), e.GetX(1), e.GetY(0), e.GetY(1));
                            var scale = (distance - m_PreviousDistance) / this.DispDistance();
                            m_PreviousDistance = distance;
                            scale += 1;
                            scale = scale * scale;
                            this.ZoomTo(scale, m_Width / 2, m_Height / 2);

                            this.Cutting();
                        }
                        else if (!m_IsScaling)
                        {
                            var distanceX = m_PreviousMoveX - (int)e.GetX();
                            var distanceY = m_PreviousMoveY - (int)e.GetY();
                            m_PreviousMoveX = (int)e.GetX();
                            m_PreviousMoveY = (int)e.GetY();

                            m_Matrix.PostTranslate(-distanceX, -distanceY);
                            this.Cutting();
                        }
                    }

                    MoveMarker();

                    break;
                case MotionEventActions.Up:


                case MotionEventActions.Pointer1Up:
                case MotionEventActions.Pointer2Up:
                    {
                        if (touchCount <= 1)
                        {
                            m_IsScaling = false;
                        }
                    }
                    break;
            }
            return true;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            return OnTouchEvent(e);
        }
    }
}