using System;
using System.Data;
using System.Web;
using System.IO;
using System.Drawing;

namespace Thumbs
{
    /// <summary>
    ///ThumbsJpg 的摘要说明
    /// </summary>
    public class ThumbsJpg
    {
        #region 属性

        private int _width;  //宽
        private int _height; //高
        private string _pic; //图片路径
        private string _type;
        private bool _result; //图片是否存在
        private string _bgcolor;


        public int width
        {
            get { return _width; }
            set { _width = value; }
        }
        public int height
        {
            get { return _height; }
            set { _height = value; }
        }
        public string pic
        {
            get { return _pic; }
            set { _pic = value; }
        }
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }
        public string bgcolor
        {
            get { return _bgcolor; }
            set { _bgcolor = value; }
        }
        public bool result
        {
            get { return _result; }
            set { _result = value; }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">原图片相对路径</param>
        /// <param name="outpath">缩略图的相对路径文件夹</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="thumbstype">缩略类型类型(HW,W,H,Cut)</param>
        public ThumbsJpg(int width, int height, string thumbstype, string backgroundcolor)
        {
            _width = width;
            _height = height;
            _type = thumbstype;
            _bgcolor = backgroundcolor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">原图片相对路径</param>
        /// <param name="outpath">缩略图的相对路径文件夹</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="thumbstype">缩略类型类型(HW,W,H,Cut,Stuff)</param>
        public ThumbsJpg(int width, int height, string thumbstype)
        {
            _width = width;
            _height = height;
            _type = thumbstype;
        }
        public void AddThumbs(string path, string outpath)
        {
            try
            {
                string temppath = PathChang(path.Replace("../../", "/"), outpath.Replace("../../", "/"));
                string severpath =HttpContext.Current.Server.MapPath(path.Replace("../../", "/"));

                string outseverpath = HttpContext.Current.Server.MapPath(temppath);
                if (Exist(temppath))
                {
                    //存在
                    _pic = temppath;
                }
                else
                {

                    switch (_type)
                    {
                        case "HW":
                            MakeThumNail(severpath, outseverpath, _width, _height, "HW");
                            this.pic = temppath;
                            break;
                        case "W":
                            MakeThumNail(severpath, outseverpath, _width, _height, "W");
                            this.pic = temppath;
                            break;
                        case "H":
                            MakeThumNail(severpath, outseverpath, _width, _height, "H");
                            this.pic = temppath;
                            break;
                        case "Cut":
                            MakeThumNail(severpath, outseverpath, _width, _height, "Cut");
                            this.pic = temppath;
                            break;
                        case "Stuff":
                            MakeStuff(severpath, outseverpath, _width, _height, _bgcolor);
                            this.pic = temppath;
                            break;
                    }
                }
            }
            catch
            {
                _pic = "0";
            }
        }
        private string PathChang(string path, string outpath)
        {
            string filename = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            return outpath + "t" + filename + "_" + _width.ToString() + _height.ToString() + extension;
        }
        private bool Exist(string pic)
        {
            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(pic)))  //图路径
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">原来图片的绝对路径及文件名</param>
        /// <param name="thumbnailPath">缩略图保存的绝对路径及文件名</param>
        /// <param name="width">缩略图长度</param>
        /// <param name="height">缩略图宽度</param>
        /// <param name="backgroundcolor">填充的背景色</param>
        public static void MakeStuff(string originalImagePath, string thumbnailPath, int width, int height, string backgroundcolor)
        {
            //获取原始图片
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            //缩略图画布宽高
            int towidth = width;
            int toheight = height;
            //原始图片写入画布坐标和宽高(用来设置裁减溢出部分)
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            //原始图片画布,设置写入缩略图画布坐标和宽高(用来原始图片整体宽高缩放)
            int bg_x = 0;
            int bg_y = 0;
            int bg_w = towidth;
            int bg_h = toheight;
            //倍数变量
            double multiple = 0;
            //获取宽长的或是高长与缩略图的倍数
            if (originalImage.Width >= originalImage.Height)
                multiple = (double)originalImage.Width / (double)width;
            else
                multiple = (double)originalImage.Height / (double)height;
            //上传的图片的宽和高小等于缩略图
            if (ow <= width && oh <= height)
            {
                //缩略图按原始宽高
                bg_w = originalImage.Width;
                bg_h = originalImage.Height;
                //空白部分用背景色填充
                bg_x = Convert.ToInt32(((double)towidth - (double)ow) / 2);
                bg_y = Convert.ToInt32(((double)toheight - (double)oh) / 2);
            }
            //上传的图片的宽和高大于缩略图
            else
            {
                //宽高按比例缩放
                bg_w = Convert.ToInt32((double)originalImage.Width / multiple);
                bg_h = Convert.ToInt32((double)originalImage.Height / multiple);
                //空白部分用背景色填充
                bg_y = Convert.ToInt32(((double)height - (double)bg_h) / 2);
                bg_x = Convert.ToInt32(((double)width - (double)bg_w) / 2);
            }
            //新建一个bmp图片,并设置缩略图大小.
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并设置背景色
            g.Clear(System.Drawing.ColorTranslator.FromHtml(backgroundcolor));
            //在指定位置并且按指定大小绘制原图片的指定部分
            //第一个System.Drawing.Rectangle是原图片的画布坐标和宽高,第二个是原图片写在画布上的坐标和宽高,最后一个参数是指定数值单位为像素
            g.DrawImage(originalImage, new System.Drawing.Rectangle(bg_x, bg_y, bg_w, bg_h), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);
            try
            {
                //获取图片类型
                string fileExtension = System.IO.Path.GetExtension(originalImagePath).ToLower();
                //按原图片类型保存缩略图片,不按原格式图片会出现模糊,锯齿等问题.
                switch (fileExtension)
                {
                    case ".gif": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Gif); break;
                    case ".jpg": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
                    case ".bmp": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Bmp); break;
                    case ".png": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png); break;
                    default: bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="orginalImagePat">原图片地址</param>
        /// <param name="thumNailPath">缩略图地址</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="model">生成缩略的模式</param>
        public static void MakeThumNail(string originalImagePath, string thumNailPath, int width, int height, string model)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int thumWidth = width;      //缩略图的宽度
            int thumHeight = height;    //缩略图的高度

            int x = 0;
            int y = 0;

            int originalWidth = originalImage.Width;    //原始图片的宽度
            int originalHeight = originalImage.Height;  //原始图片的高度

            switch (model)
            {
                case "HW":      //指定高宽缩放,可能变形
                    break;
                case "W":       //指定宽度,高度按照比例缩放
                    thumHeight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H":       //指定高度,宽度按照等比例缩放
                    thumWidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut":
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)thumWidth / (double)thumHeight)
                    {
                        originalHeight = originalImage.Height;
                        originalWidth = originalImage.Height * thumWidth / thumHeight;
                        y = 0;
                        x = (originalImage.Width - originalWidth) / 2;
                    }
                    else
                    {
                        originalWidth = originalImage.Width;
                        originalHeight = originalWidth * height / thumWidth;
                        x = 0;
                        y = (originalImage.Height - originalHeight) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(thumWidth, thumHeight);

            //新建一个画板
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量查值法
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            //设置高质量，低速度呈现平滑程度
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            graphic.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            graphic.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, thumWidth, thumHeight), new System.Drawing.Rectangle(x, y, originalWidth, originalHeight), System.Drawing.GraphicsUnit.Pixel);

            try
            {
                bitmap.Save(thumNailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                graphic.Dispose();
            }
        }

        /// <summary>
        /// 在图片上添加文字水印
        /// </summary>
        /// <param name="path">要添加水印的图片路径</param>
        /// <param name="syPath">生成的水印图片存放的位置</param>
        public static void AddWaterWord(string path, string syPath)
        {
            string syWord = "http://www.hello36.cn";
            System.Drawing.Image image = System.Drawing.Image.FromFile(path);

            //新建一个画板
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(image);
            graphic.DrawImage(image, 0, 0, image.Width, image.Height);

            //设置字体
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 60);

            //设置字体颜色
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Green);

            graphic.DrawString(syWord, f, b, 35, 35);
            graphic.Dispose();

            //保存文字水印图片
            image.Save(syPath);
            image.Dispose();

        }

        /// <summary>
        /// 在图片上添加图片水印
        /// </summary>
        /// <param name="path">原服务器上的图片路径</param>
        /// <param name="syPicPath">水印图片的路径</param>
        /// <param name="waterPicPath">生成的水印图片存放路径</param>
        public static void AddWaterPic(string path, string syPicPath, string waterPicPath)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(path);
            System.Drawing.Image waterImage = System.Drawing.Image.FromFile(syPicPath);
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(image);
            graphic.DrawImage(waterImage, new System.Drawing.Rectangle(image.Width - waterImage.Width, image.Height - waterImage.Height, waterImage.Width, waterImage.Height), 0, 0, waterImage.Width, waterImage.Height, System.Drawing.GraphicsUnit.Pixel);
            graphic.Dispose();

            image.Save(waterPicPath);
            image.Dispose();
        }
    }
}
