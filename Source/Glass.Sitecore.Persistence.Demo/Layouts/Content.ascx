<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Content.ascx.cs" Inherits="Glass.Sitecore.Persistence.Demo.Layouts.Content" %>
<div>
    <h1>
        <asp:Literal runat="server" ID="title" /></h1>
    <div class="body">
        <asp:Literal runat="server" ID="body" />
    </div>
    <div class="links">
        <asp:Repeater runat="server" ID="links">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                    <a href='<%# DataBinder.Eval(Container.DataItem,"Url") %>'><%# DataBinder.Eval(Container.DataItem,"Title") %></a>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>
