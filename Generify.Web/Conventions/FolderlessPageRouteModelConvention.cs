using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;

namespace Generify.Web.Conventions
{
    public class FolderlessPageRouteModelConvention : IPageRouteModelConvention
    {
        public void Apply(PageRouteModel model)
        {
            foreach (SelectorModel selector in model.Selectors)
            {
                string[] segments = selector.AttributeRouteModel.Template.Split('/');

                if (string.IsNullOrWhiteSpace(model.AreaName))
                {
                    selector.AttributeRouteModel.Template = segments.Last();
                }
                else
                {
                    selector.AttributeRouteModel.Template = model.AreaName.Trim('/') + "/" + segments.Last();
                }
            }
        }
    }
}
