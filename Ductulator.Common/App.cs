using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Windows.Media.Imaging;
using System.IO;
using Ductulator.Common.Service;
using Ductulator.Common.Utils;
using System.Windows.Media;
using System.Windows;
using Autodesk.Windows;
using System.Linq;

namespace Ductulator
{ 
    class App : Autodesk.Revit.UI.IExternalApplication
    {
        public static RevitCollectorService RevitCollectorService;
        private static readonly string RIBBONTAB = "e-verse";
        private static readonly string RIBBONPANEL = "Ductulator";
        private static readonly string BUTTONNAME = "Nancy";

        // Generate an Guid for the App
        static AddInId m_appId = new AddInId(new Guid(
        "FB8E8820-DB2B-49E5-A298-11FDFB710910"));

        // Get the absolute path of this assembly.
        static string ExecutingAssemblyPath = System.Reflection.Assembly
          .GetExecutingAssembly().Location;

        public static string typeDuct = "";

        public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
        {
            RevitCollectorService = new RevitCollectorService(application.GetUIApplication());

            Autodesk.Windows.RibbonControl ribbon = Autodesk.Windows.ComponentManager.Ribbon;
            Autodesk.Windows.RibbonTab tab =
            ribbon.Tabs.FirstOrDefault(tabAbout => tabAbout.Id.Contains(RIBBONTAB));

            if (tab == null)
            {
                CreateRibbonTab(application, RIBBONTAB);
            }

            tab = ribbon.Tabs.FirstOrDefault(tabAbout => tabAbout.Id.Contains(RIBBONTAB));


            Autodesk.Revit.UI.RibbonPanel ribbonPanel = application.CreateRibbonPanel(RIBBONTAB, RIBBONPANEL);

            PushButton pushButton = ribbonPanel.AddItem(new PushButtonData(BUTTONNAME,
                BUTTONNAME, ExecutingAssemblyPath, "Ductulator.MainCommand")) as PushButton;

            pushButton.ToolTip = "Nancy - Ductulator";

            pushButton.LongDescription =
             "Properly re-size your ducts and then modify it base on the new dimensions" +
             " , you can also convert rectangular ducts into round and viceversa.";

            string logoPath = "M111.1 29.5L29 74.2v86.4l80.9 52.9l80.1-45.8V81.4L111.1 29.5z M181.3 82.8l-71.2 40.7L37.6 76.4l73.3-39.9 L181.3 82.8z M35 81.9l72 46.8v75.8l-72-47.1V81.9z M113 204.8v-76.1l71-40.6v76.1L113 204.8z M134 173.3l31-18.6v-36.8l-31 17.4V173.3z M140 142.7l16.7 10l-16.7 10V142.7z M159 147l-16.4-9.7l16.4-9.2 V147z";
            pushButton.LargeImage = CreateLogo(logoPath);

            ContextualHelp contexHelp = new ContextualHelp(ContextualHelpType.Url, Links.ductulatorWebsite);
            pushButton.SetContextualHelp(contexHelp);


            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private BitmapSource CreateLogo(string logoPath, double size = 25)
        {
            Geometry pathGeometry = PathGeometry.Parse(logoPath);
            Rect bounds = pathGeometry.Bounds;

            double scale = Math.Min(size / bounds.Width, size / bounds.Height);

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(scale, scale));
            transformGroup.Children.Add(new TranslateTransform(
                -bounds.X * scale + (size - bounds.Width * scale) / 2,
                -bounds.Y * scale + (size - bounds.Height * scale) / 2));

            GeometryDrawing drawing = new GeometryDrawing
            {
                Geometry = pathGeometry,
                Brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(249, 79, 70)),
                Pen = null
            };

            DrawingGroup drawingGroup = new DrawingGroup
            {
                Transform = transformGroup
            };
            drawingGroup.Children.Add(drawing);

            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext context = drawingVisual.RenderOpen())
            {
                context.DrawDrawing(drawingGroup);
            }

            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)size, (int)size, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(drawingVisual);
            bitmap.Freeze();

            return bitmap;
        }

        public static void CreateRibbonTab(UIControlledApplication application, string ribbonTabName)
        {
            RibbonControl ribbon = ComponentManager.Ribbon;
            RibbonTab tab = ribbon.FindTab(ribbonTabName);

            if (tab == null)
            {
                application.CreateRibbonTab(ribbonTabName);
            }
        }
    }
}