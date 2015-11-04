using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

namespace Tausendblatt
{
    public enum LogoDetail
    {
        Header,
        Hill,
        OriginalSee,
        OriginalMeadow,
        See,
        Meadow,
        Street,
        House7,
        House5,
        House,
        Sun,
    }

    public partial class PageTausendblatt : ContentPage
    {
        public PageTausendblatt()
        {
            InitializeComponent();

            webViewInfo.Navigating += (object sender, WebNavigatingEventArgs e) =>
            {
                // Analysis disable StringStartsWithIsCultureSpecific
                if (Uri.IsWellFormedUriString(e.Url, UriKind.Absolute) && !e.Url.StartsWith("file"))
                {
                    Device.OpenUri(new Uri(e.Url));
                    e.Cancel = true;
                }
                // Analysis restore StringStartsWithIsCultureSpecific
            };
        }

        protected  override void OnAppearing()
        {
            base.OnAppearing();
            Run();
        }

        #region ToolbarItemRepeat

        ToolbarItem ToolbarItemRepeat = null;

        bool ToolbarItemRepeatIsVisible
        {
            get
            {
                return ToolbarItemRepeat != null && ToolbarItems.Contains(ToolbarItemRepeat);
            }
            set
            {
                try
                {
                    if (value)
                    {
                        if (ToolbarItemRepeat == null)
                            ToolbarItemRepeat = new ToolbarItem("Repeat", "Icons/Repeat.png", Run);
                        if (!ToolbarItemRepeatIsVisible)
                            ToolbarItems.Add(ToolbarItemRepeat);
                    }
                    else
                    {
                        if (ToolbarItemRepeatIsVisible)
                            ToolbarItems.Remove(ToolbarItemRepeat);
                    }
                }
                catch
                {
                }
            }
        }

        #endregion

        Image ImageLogo;
        Image ImageLogoFull;
        Dictionary<LogoDetail, Image> DetailImages = new Dictionary<LogoDetail, Image>();
        Dictionary<LogoDetail, Label> DetailLabels = new Dictionary<LogoDetail, Label>();
        Rectangle ImageLogoBackgroundBounds = Rectangle.Zero;
        Rectangle ImageDetailBounds, ImagesDetailBoundsLeftOut, ImagesDetailBoundsRightOut, ImagesDetailBoundsTopOut, ImagesDetailBoundsTopOutHalf = Rectangle.Zero;

        async void Run()
        {
            try
            {
                await Build();
                await Animate();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        async Task Build()
        {
            // hide all
            await boxViewCanvas.FadeTo(1, 0);

            StopAnimationLoop();
            await Task.Delay(1000);

            // Background;
            imageLogoBackground.Source = ImageSourceFromResource("LogoBackground");
            imageLogoBackground.IsVisible = false;
            ImageLogoBackgroundBounds = Rectangle.Zero;
  
            // Images 
            absoluteLayoutImages.Children.Clear();
            ImageLogo = AddImage("LogoTransparent", false);
            ImageLogoFull = AddImage("LogoFullTransparent", false);
            AddDetailImage(LogoDetail.Hill, "LogoTransparentHill");
            AddDetailImage(LogoDetail.OriginalSee, "LogoTransparentOriginalSee");
            AddDetailImage(LogoDetail.OriginalMeadow, "LogoTransparentOriginalMeadow");
            AddDetailImage(LogoDetail.See, "LogoTransparentSee");
            AddDetailImage(LogoDetail.Meadow, "LogoTransparentMeadow");
            AddDetailImage(LogoDetail.Street, "LogoTransparentStreet");
            AddDetailImage(LogoDetail.House7, "LogoTransparentHouse7");
            AddDetailImage(LogoDetail.House5, "LogoTransparentHouse5");
            AddDetailImage(LogoDetail.House, "LogoTransparentHouse");
            AddDetailImage(LogoDetail.Sun, "LogoTransparentSun");

            await Task.Delay(500);

            // Prepaire
            ImageDetailBounds = new Rectangle(DetailImages[LogoDetail.House].Bounds.X, DetailImages[LogoDetail.House].Bounds.Y, DetailImages[LogoDetail.House].Bounds.Width, DetailImages[LogoDetail.House].Bounds.Height);
            ImageDetailBounds.Height = ImageDetailBounds.Width;

            ImagesDetailBoundsLeftOut = new Rectangle(-ImageDetailBounds.Width, ImageDetailBounds.Y, ImageDetailBounds.Width, ImageDetailBounds.Height);
            ImagesDetailBoundsRightOut = new Rectangle(absoluteLayoutImages.Width, ImageDetailBounds.Y, ImageDetailBounds.Width, ImageDetailBounds.Height);

            var x = (absoluteLayoutImages.Width - ImageDetailBounds.Width) / 2;
            ImagesDetailBoundsTopOut = new Rectangle(x, -ImageDetailBounds.Height, ImageDetailBounds.Width, ImageDetailBounds.Height);
            ImagesDetailBoundsTopOutHalf = new Rectangle(x, -ImageDetailBounds.Height / 2, ImageDetailBounds.Width, ImageDetailBounds.Height);

            await Task.WhenAll(new Task[]
                {
                    imageLogoBackground.ScaleTo(0, 0),
                    imageLogoBackground.FadeTo(0, 0),
                    ImageLogo.FadeTo(0, 0),
                    ImageLogoFull.FadeTo(0, 0),

                    DetailImages[LogoDetail.Hill].LayoutTo(ImagesDetailBoundsTopOut, 0),
                    DetailImages[LogoDetail.OriginalSee].LayoutTo(ImagesDetailBoundsLeftOut, 0),
                    DetailImages[LogoDetail.OriginalMeadow].LayoutTo(ImagesDetailBoundsRightOut, 0),
                    DetailImages[LogoDetail.See].LayoutTo(ImagesDetailBoundsRightOut, 0),
                    DetailImages[LogoDetail.Meadow].LayoutTo(ImagesDetailBoundsLeftOut, 0),
                    DetailImages[LogoDetail.Street].FadeTo(0, 0),
                    DetailImages[LogoDetail.House].FadeTo(0, 0),
                    DetailImages[LogoDetail.House7].LayoutTo(ImagesDetailBoundsTopOutHalf, 0),
                    DetailImages[LogoDetail.House5].LayoutTo(ImagesDetailBoundsTopOutHalf, 0),
                    DetailImages[LogoDetail.Sun].FadeTo(0, 0),
                });

            // Labels 
            stackLayoutInfo.Children.Clear();
            AddDetailLabel(LogoDetail.Header, Color.Black, 20);
            AddDetailLabel(LogoDetail.Hill, Styles.Colors.Hill);
            AddDetailLabel(LogoDetail.See, Styles.Colors.See);
            AddDetailLabel(LogoDetail.Meadow, Styles.Colors.Meadow);
            AddDetailLabel(LogoDetail.Street, Styles.Colors.Street);
            AddDetailLabel(LogoDetail.House, Styles.Colors.House);

            WebViewIsVisible = false;
        }

        async Task Animate()
        {
            ToolbarItemRepeatIsVisible = false;
            try
            {
                #region Animation Details

                #region Start
                const uint animation = 2000;
                uint animationHalf = animation / 2;
                uint animationDouble = animation * 2;

                await boxViewCanvas.FadeTo(0, 0);
                #endregion

                #region Original
                DetailLabels[LogoDetail.Header].Text = "vor 1980";
                DetailLabels[LogoDetail.Hill].Text = "Schmalberg";
                DetailLabels[LogoDetail.See].Text = "Rehbach";
                DetailLabels[LogoDetail.Meadow].Text = "Flur Schäferheck (Sumpfgebiet)";

                await Task.WhenAll(new Task[]
                    {
                        DetailImages[LogoDetail.Hill].LayoutTo(ImageDetailBounds, animation, Easing.BounceOut),
                        DetailImages[LogoDetail.OriginalSee].LayoutTo(ImageDetailBounds, animation, Easing.CubicIn),
                        DetailImages[LogoDetail.OriginalMeadow].LayoutTo(ImageDetailBounds, animation, Easing.CubicIn),
                    });

                await Task.Delay((int)animation);
                #endregion

                #region Waldsee
                DetailLabels[LogoDetail.Header].Text = "Spatenstich 1980";
                DetailLabels[LogoDetail.Hill].Text = "Schmalberg";
                DetailLabels[LogoDetail.See].Text = "Waldsee gespeist vom Rehbach";
                DetailLabels[LogoDetail.Meadow].Text = "Liegewiese";

                await Task.WhenAll(new Task[]
                    {
                        DetailImages[LogoDetail.OriginalSee].LayoutTo(ImagesDetailBoundsRightOut, animation, Easing.CubicOut),
                        DetailImages[LogoDetail.OriginalSee].FadeTo(0, animation, Easing.SpringIn),
                        DetailImages[LogoDetail.OriginalMeadow].LayoutTo(ImagesDetailBoundsLeftOut, animation, Easing.CubicOut),
                        DetailImages[LogoDetail.OriginalMeadow].FadeTo(0, animation, Easing.SpringIn),
                        DetailImages[LogoDetail.See].LayoutTo(ImageDetailBounds, animation, Easing.CubicIn),
                        DetailImages[LogoDetail.Meadow].LayoutTo(ImageDetailBounds, animation, Easing.CubicIn),
                    });
                await Task.Delay((int)animation);
                #endregion

                #region Street and House
                DetailLabels[LogoDetail.Header].Text = "2004";
                DetailLabels[LogoDetail.Street].Text = "Panoramaterrasse";

                await DetailImages[LogoDetail.Street].FadeTo(1, animation);

                DetailLabels[LogoDetail.Header].Text = "2013";
                DetailLabels[LogoDetail.House].Text = "Tausendblatt 7";

                await Task.WhenAny(new Task[]
                    {
                        DetailImages[LogoDetail.House7].LayoutTo(ImageDetailBounds, animation, Easing.BounceOut),
                        Task.Delay((int)animation / 4),
                    });

                DetailLabels[LogoDetail.Header].Text = "2015";
                DetailLabels[LogoDetail.House].Text = "Tausendblatt 7 und 5";

                await DetailImages[LogoDetail.House5].LayoutTo(ImageDetailBounds, animation, Easing.BounceOut);

                await Task.WhenAny(new Task[]
                    {
                        DetailImages[LogoDetail.House7].FadeTo(0, animation),
                        DetailImages[LogoDetail.House7].LayoutTo(new Rectangle(ImageDetailBounds.X + 10, ImageDetailBounds.Y, ImageDetailBounds.Width, ImageDetailBounds.Height), animationDouble),
                        DetailImages[LogoDetail.House5].FadeTo(0, animation),
                        DetailImages[LogoDetail.House5].LayoutTo(new Rectangle(ImageDetailBounds.X - 10, ImageDetailBounds.Y, ImageDetailBounds.Width, ImageDetailBounds.Height), animationDouble),
                        DetailImages[LogoDetail.House].FadeTo(1, animation),
                        Task.Delay((int)animationHalf / 2),
                    });
                await DetailImages[LogoDetail.Sun].FadeTo(1, animation, Easing.SinOut);
                #endregion
                
                #endregion

                #region Finsihed Logo
                ImageLogo.IsVisible = true;
                await ImageLogo.FadeTo(1, 0);
                DetailImagesIsVisible = false;
          
                await ImageLogo.ScaleTo(1.5, animation, Easing.Linear);
                await Task.Delay((int)animation);

                await Task.WhenAll(new Task[]
                    {
                        DetailLabelsFadeTo(0, animation, Easing.Linear),
                        ImageLogo.ScaleTo(0, animation, Easing.Linear),
                        ImageLogo.RotateTo(3600, animation, Easing.Linear), 
                        ImageLogo.FadeTo(0, animation, Easing.Linear)
                    });

                ImageLogo.IsVisible = false;
    
                ImageLogoFull.IsVisible = true;

                await Task.WhenAll(new Task[]
                    {
                        ImageLogoFull.RotateTo(0, 0),
                        ImageLogoFull.FadeTo(1, animation),
                        ImageLogoFull.ScaleTo(1.4, animation, Easing.SinIn),
                    });

                await Task.Delay((int)animationHalf);

                await webViewInfo.FadeTo(0, 0);
                WebViewIsVisible = true;
                webViewInfo.Source = new HtmlWebViewSource
                {
                    Html = "<h1>Ferienhaus Tausendblatt</h1></br>Logo animation for the Tausendblatt app</br></br>Code and description on <a href=\"https://github.com/EifelMono/Tausendblatt\">github</a>"
                };

                const int fak = 4;
                await Task.WhenAny(new Task[]
                    {
                        ImageLogoFull.LayoutTo(new Rectangle(absoluteLayoutImages.Width / 20, absoluteLayoutImages.Height / 20, ImageLogoFull.Bounds.Width / fak, ImageLogoFull.Bounds.Height / fak), animation, Easing.SinIn),
                        Task.Delay((int)animationHalf),
                        webViewInfo.FadeTo(1, animationDouble)
                    });


                AnimationLoop();
             
                #endregion
            }
            finally
            {
                ToolbarItemRepeatIsVisible = true;
            }
        }

        #region Animation Loop and more

        bool IsAnimationLoopRunning = false;

        void StopAnimationLoop()
        {
            try
            {
                IsAnimationLoopRunning = false;
                ViewExtensions.CancelAnimations(imageLogoBackground);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        async void AnimationLoop()
        {
            imageLogoBackground.IsVisible = true;
            const uint loopAnimation = 15000;

            if (ImageLogoBackgroundBounds == Rectangle.Zero)
                ImageLogoBackgroundBounds = new Rectangle(imageLogoBackground.Bounds.X, imageLogoBackground.Bounds.Y, imageLogoBackground.Bounds.Width, imageLogoBackground.Bounds.Height);

            var yOffset = ImageLogoBackgroundBounds.Height / 4;
            var xOffset = ImageLogoBackgroundBounds.Width / 50;

            await imageLogoBackground.LayoutTo(new Rectangle(0, 0, 0, 0), 0);

            await Task.WhenAll(new Task[]
                {
                    imageLogoBackground.FadeTo(1, loopAnimation / 10),
                    imageLogoBackground.ScaleTo(1.4, loopAnimation / 10),
                    imageLogoBackground.LayoutTo(new Rectangle(ImageLogoBackgroundBounds.X, ImageLogoBackgroundBounds.Y - yOffset, ImageLogoBackgroundBounds.Width, ImageLogoBackgroundBounds.Height), loopAnimation / 10)
                });

            IsAnimationLoopRunning = true;
            while (IsAnimationLoopRunning)
            {
                await Task.WhenAll(new Task[]
                    {
                        imageLogoBackground.ScaleTo(1.3, loopAnimation),
                        imageLogoBackground.LayoutTo(new Rectangle(ImageLogoBackgroundBounds.X + xOffset, ImageLogoBackgroundBounds.Y - yOffset, ImageLogoBackgroundBounds.Width, ImageLogoBackgroundBounds.Height), loopAnimation)
                    });
                if (!IsAnimationLoopRunning)
                    break;
                await Task.WhenAll(new Task[]
                    {
                        imageLogoBackground.LayoutTo(new Rectangle(ImageLogoBackgroundBounds.X - xOffset, ImageLogoBackgroundBounds.Y - yOffset, ImageLogoBackgroundBounds.Width, ImageLogoBackgroundBounds.Height), loopAnimation),
                        imageLogoBackground.ScaleTo(1.4, loopAnimation)
                    });
                if (!IsAnimationLoopRunning)
                    break;
            }
        }

        #endregion

        #region Helpers

        ImageSource ImageSourceFromResource(string resourceName)
        {
            return ImageSource.FromResource(string.Format("Tausendblatt.Resources.{0}.png", resourceName));
        }

        Image AddImage(string resourceName, bool isVisible = true)
        {
            var w = absoluteLayoutImages.Bounds.Width;
            var h = absoluteLayoutImages.Bounds.Height;
            var xOffset = 0.15 * w;
            var yOffset = 0.15 * h;
            var image = new Image
            {
                WidthRequest = w - 2 * xOffset,
                HeightRequest = WidthRequest,
                Source = ImageSourceFromResource(resourceName),
                Aspect = Aspect.AspectFit,
                IsVisible = isVisible,
            };
            absoluteLayoutImages.Children.Add(image, new Point(xOffset, yOffset));
            return image;
        }

        void AddDetailImage(LogoDetail detailImage, string resourceName)
        {
            DetailImages[detailImage] = AddImage(resourceName);
        }

        bool DetailImagesIsVisible
        {
            set
            {
                foreach (var kv in DetailImages)
                {
                    kv.Value.IsVisible = value;
                    if (value)
                        kv.Value.FadeTo(1, 0);
                    else
                        kv.Value.FadeTo(0, 0);
                }
            }
        }

        void AddDetailLabel(LogoDetail detail, Color color, int fontSize = 18, FontAttributes fontAttributes = FontAttributes.Bold)
        {
            var label = new Label
            {
                Text = "",
                TextColor = color,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontFamily = "HelveticaNeue-Thin",
                FontSize = fontSize,
                FontAttributes = fontAttributes
            };
            DetailLabels[detail] = label;
            stackLayoutInfo.Children.Add(label);
        }

        Task DetailLabelsFadeTo(double opacity, uint length = 250, Easing easing = null)
        {
            List<Task> tasks = new List<Task>();
            foreach (var kv in DetailLabels)
                tasks.Add(kv.Value.FadeTo(opacity, length, easing));
            return Task.WhenAll(tasks.ToArray());
        }

        bool WebViewIsVisible
        {
            get
            {
                return webViewInfo.IsVisible;
            }
            set
            {
                boxViewSplit.IsVisible = value;
                boxViewInfo.IsVisible = value;
                webViewInfo.IsVisible = value;
            }
        }

        #endregion
    }
}



