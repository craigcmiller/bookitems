<%@ Page Title="" Language="C#" MasterPageFile="~/ExternalSite.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BookItems.Site.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Image ID="gcgfgImage" runat="server" ImageUrl="~/Images/gcgfg_homepage.jpg" />

    <p>
        Enter your user name and password to make a booking
    </p>

    <asp:Label ForeColor="Red" runat="server" ID="loginErrorLabel" />

    <div id="loginArea" align="center">
        <table border="0">
            <tr>
                <td>Pilot:</td><td><asp:TextBox runat="server" ID="userNameTextBox"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Password:</td><td><asp:TextBox runat="server" TextMode="Password" ID="passwordTextBox"></asp:TextBox></td>
            </tr>
            <tr>
                <td></td><td align="right"><asp:Button runat="server" Text="Login" 
                    ID="loginButton" onclick="loginButton_Click" /></td>
            </tr>
        </table>
    </div>
</asp:Content>
