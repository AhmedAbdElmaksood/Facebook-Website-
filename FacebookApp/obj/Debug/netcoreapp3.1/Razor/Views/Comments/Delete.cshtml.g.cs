#pragma checksum "D:\ITI\MVC\Project\NICKNAME\17. Ajax with photo\FacebookApp\Views\Comments\Delete.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7e59ac625596e02129077fe5ba087352777acfda"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Comments_Delete), @"mvc.1.0.view", @"/Views/Comments/Delete.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\ITI\MVC\Project\NICKNAME\17. Ajax with photo\FacebookApp\Views\_ViewImports.cshtml"
using FacebookApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\ITI\MVC\Project\NICKNAME\17. Ajax with photo\FacebookApp\Views\_ViewImports.cshtml"
using FacebookApp.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7e59ac625596e02129077fe5ba087352777acfda", @"/Views/Comments/Delete.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9ce7d7be762a95bf1d1f2de044e07ff6a15152ae", @"/Views/_ViewImports.cshtml")]
    public class Views_Comments_Delete : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<FacebookApp.Models.UserCommentsOnPost>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n<div>\r\n    <div class=\"modal fade\" id=\"deleteCommentModal\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"deleteModalLabel\" aria-hidden=\"true\">\r\n        <div class=\"modal-dialog\" role=\"document\">\r\n            <div class=\"modal-content\">\r\n");
            WriteLiteral(@"                <div class=""modal-body"">
                    Are you sure you want to delete this Comment?
                </div>
                <div class=""modal-footer"">
                    <button type=""button"" class=""btn btn-secondary"" data-dismiss=""modal"">Cancel</button>
                    <input type=""button"" value=""Yes"" class=""btn btn-primary""");
            BeginWriteAttribute("onclick", " onclick=\"", 855, "\"", 897, 3);
            WriteAttributeValue("", 865, "deleteComment(", 865, 14, true);
#nullable restore
#line 17 "D:\ITI\MVC\Project\NICKNAME\17. Ajax with photo\FacebookApp\Views\Comments\Delete.cshtml"
WriteAttributeValue("", 879, Model.CommentId, 879, 16, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 895, ");", 895, 2, true);
            EndWriteAttribute();
            WriteLiteral(" />\r\n                </div>\r\n");
            WriteLiteral(@"            </div>

        </div>
    </div>

</div>

<script>
        function deleteComment(CommentId) {

        $.ajax({
                type: 'POST',
                url: '/Comments/Delete',
            data: {
                CommentId:CommentId,
                },
                dataType: 'json',
                success: function (response) {
                    refreshPost();
                    $('#deleteCommentModal').modal('toggle');
                },

                error: function (response) {
                    alert(""Error"");
                }
            });
        }

</script>

");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<FacebookApp.Models.UserCommentsOnPost> Html { get; private set; }
    }
}
#pragma warning restore 1591
