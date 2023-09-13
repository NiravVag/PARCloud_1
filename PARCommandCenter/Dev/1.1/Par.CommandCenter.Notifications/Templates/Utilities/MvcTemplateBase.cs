//using RazorEngine.Templating;
//using RazorEngine.Text;
//using System;
//using System.Diagnostics.CodeAnalysis;
//using System.IO;

//namespace Utilities
//{
//    public abstract class MvcTemplateBase<T> : TemplateBase<T>
//    {
//        public IEncodedString EmbedCss([NotNull] string path)
//        {
//            // load the contents of that file
//            string cssText;
//            try
//            {
//                cssText = File.ReadAllText(path);
//            }
//            catch (Exception)
//            {
//                // blank string if we can't read the file for any reason
//                cssText = string.Empty;
//            }

//            return this.Raw(cssText);
//        }


//        public TemplateWriter RazorPartial(string template)
//        {
//            return this.Include(template);
//        }
//    }
//}
