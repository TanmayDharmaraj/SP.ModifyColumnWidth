<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeColumnWidth.aspx.cs" Inherits="ChangeColumnWidth.Layouts.ChangeColumnWidth.ChangeColumnWidth" DynamicMasterPageFile="~masterurl/default.master" %>


<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    
    <asp:Repeater ID="Repeater1" runat="server">
        <HeaderTemplate>
            <table>
                <tr>
                    <td>Column Name</td>
                    <td>Width</td>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><asp:Label runat="server"  ID="label1" AssociatedControlID="txt1" Text='<%#Eval("ViewFields") %>'></asp:Label></td>
                <td><asp:TextBox ID="txt1" runat="server" data-attribute='<%#Eval("ViewFields") %>'></asp:TextBox>
                    </td>
                <td><asp:Literal ID="colMeasure" runat="server" Text="px"></asp:Literal></td>
            </tr>
            <tr>
                <td colspan="3" align="right">
                    <asp:RegularExpressionValidator 
                        ID="RegularExpressionValidator1" 
                        runat="server" 
                        ErrorMessage="Only numbers allowed"
                        ControlToValidate="txt1"
                        EnableClientScript="true"
                        Display="Dynamic"
                        ForeColor="#ff5050"
                        ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>

                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            <tr>
                <td style="margin-top:10px" colspan="3" valign="middle" align="right">
                    <asp:Button ID="btnOk" runat="server" Text="Ok" OnClick="btnOk_Click" />
                </td>
            </tr>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Label runat="server" ID="lblOutput" Text=""></asp:Label>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
