using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChangeColumnWidth.Layouts.ChangeColumnWidth
{
    public partial class ChangeColumnWidth : LayoutsPageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
               
                /*To implement later. 
                 * The below code searches through the existing script file and populates the textboxes of the repeater
                 * with values that have been set before
                 * The code is highly dependent on the script being unedited by the user after insertion.
                 * Hence temporaily on-hold untill a better solution is found
                 * 
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ServerRelativeUrl))
                    {
                        SPFile page = web.GetFile(Request.QueryString["PagePath"]);
                        SPLimitedWebPartManager wpManager = page.GetLimitedWebPartManager(System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared);
                        //remove the old wepart.
                        IList<System.Web.UI.WebControls.WebParts.WebPart> _listFormWebParts = (from System.Web.UI.WebControls.WebParts.WebPart webPart in wpManager.WebParts
                                                                                               where webPart.Title == "Change Column Width"
                                                                                               select webPart).ToList();

                        ScriptEditorWebPart sewp = _listFormWebParts[0] as ScriptEditorWebPart;
                        
                        Hashtable hs = GetPreviousValues(sewp.Content);
                    }
                }
                */
                BindRepeater(Request.QueryString["ListId"].ToString());
            }
                
        }
        /*Heavy lifting of finding existing set values in the script depending.
         * @Param : string sewpContent
         *          The script from the inserted webpart
         * 
        private Hashtable GetPreviousValues(string sewpContent)
        {
            //Hashtable ht = new Hashtable();
            MatchCollection mc = Regex.Matches(sewpContent, @"(TH\.ms-vh2:contains\((\'|\"")\w+(\'|\"")\)(\'|\"")\)\.css\((\'|\"")width(\'|\"")\,\s*(\'|\"")\d+px(\'|\"")\))");
            foreach (Match value in mc)
            {
                string val = value.ToString();
                string[] separator = new string[]{".css"};
                string[] arr= val.Split(separator,StringSplitOptions.RemoveEmptyEntries);
                
                //get items inside ""
                MatchCollection mc1 = Regex.Matches(arr[1], @"([""'])(?:(?=(\\?))\2.)*?\1");
                Match width = mc1[1];
                int length = (width.ToString().Length)-1;
                string _width = null;
                if (width.ToString().Substring(length - 2, 2).ToLower() == "px")
                {
                    _width = width.ToString().Substring(0, length - 2);
                }
                
            }
            return null;
            
        }*/

        private void BindRepeater(string ListId)
        {
            Guid id = new Guid(ListId);
            SPList li = SPContext.Current.Web.Lists[id];
            SPView view = li.DefaultView;
            SPViewFieldCollection fields = view.ViewFields;
            DataTable dt = new DataTable();
            dt.Columns.Add("ViewFields");
            foreach (string field in fields)
            {
                SPField f = li.Fields.GetFieldByInternalName(field);
                dt.Rows.Add(f.Title);
            }
            DataView dv = new DataView(dt);

            Repeater1.DataSource = dv;
            Repeater1.DataBind();
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.AppendLine("<script type='text/javascript'>$(document).ready(function() {");
                foreach (RepeaterItem ctr in Repeater1.Items)
                {
                    foreach (Control ctrl in ctr.Controls)
                    {
                        if (ctrl is System.Web.UI.WebControls.TextBox)
                        {
                            TextBox tb = ctrl as TextBox;
                            if (!string.IsNullOrWhiteSpace(tb.Text))
                                strBuilder.AppendLine(@"$(""TH.ms-vh2:contains('" + tb.Attributes["data-attribute"] + @"')"").css(""width"", """ + tb.Text + @"px"");");
                        }
                    }
                }
                strBuilder.AppendLine("});</script>");
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                    {
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ServerRelativeUrl))
                        {

                            SPFile page = web.GetFile(Request.QueryString["PagePath"]);
                            SPLimitedWebPartManager wpManager = page.GetLimitedWebPartManager(System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared);
                            //remove the old wepart.
                            IList<System.Web.UI.WebControls.WebParts.WebPart> _listFormWebParts = (from System.Web.UI.WebControls.WebParts.WebPart webPart in wpManager.WebParts
                                                                                                   where webPart.Title == "Change Column Width"
                                                                                                   select webPart).ToList();
                            for (int i = 0; i < _listFormWebParts.Count; i++)
                            {
                                wpManager.DeleteWebPart(_listFormWebParts[i]);
                            }

                            ScriptEditorWebPart sewp = new ScriptEditorWebPart();
                            sewp.Title = "Change Column Width";
                            sewp.Content = strBuilder.ToString();
                            wpManager.AddWebPart(sewp, "Top", 0);
                        }
                    }

                });
                string script = @"SP.SOD.executeFunc('sp.js','SP.ClientContext',function(){window.frameElement.commonModalDialogClose(SP.UI.DialogResult.OK,'Web part added successfully');});";
                Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(),"StatusMessage", script,true);
            }
            catch (Exception exp)
            {
                string script = @"SP.SOD.executeFunc('sp.js','SP.ClientContext',function(){window.frameElement.commonModalDialogClose(SP.UI.DialogResult.Cancel,'Something went wrong...\n"+exp.Message+"');});";
                Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "StatusMessage", script, true);
            }
        }
    }
}
