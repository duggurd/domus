using HtmlAgilityPack;

namespace DomusRefactor;

public static class Webscraper {
    public static HtmlDocument LoadWebPageHtml(string url) {
        var web = new HtmlWeb();
        return web.Load(url);
    }
}