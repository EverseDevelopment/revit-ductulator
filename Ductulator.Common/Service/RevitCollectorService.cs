using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ductulator.Common.Service
{
    class RevitCollectorService
    {
        private UIApplication uiApplication;
        public RevitCollectorService(UIApplication uIApplication)
        {
            uiApplication = uIApplication;
        }

        public Document GetDocument()
        {
            return uiApplication.ActiveUIDocument.Document;
        }
    }
}
