using HtmlAgilityPack;

using Domus;

namespace DomusRefactor;

public class FinnRealEstateDataExtractor {
    private HtmlDocument htmlDocument { get; set; } 


    public List<RealEstate> ExtractRealEstateDataFromWebpage() {
        return ExtractDataFromArticles();
    }

    private List<RealEstate> ExtractDataFromArticles() {
        var realEstates = new List<RealEstate>();
        foreach (var realEstateArticleNode in ExtractAllArticleNodes()) {
            var realEstate = ExtractRealEstateData(realEstateArticleNode);
            realEstates.Add(realEstate);
        }
        return realEstates;
    }

    private IEnumerable<HtmlNode> ExtractAllArticleNodes() {
        return htmlDocument.DocumentNode.Descendants("article");
    }

    private RealEstate ComposeNewRealEstate(HtmlNode realEstateArticleNode) {
        var realEstate = new RealEstate() {
            address = ExtractCleanAddressFromArticleNode(realEstateArticleNode),
            finnkodeId = ExtractCleanRealEstateFinnkode(realEstateArticleNode),
            price = ExtractRealEstatePrice(realEstateArticleNode),
            squareMeters = ExtractCleanRealEstateSquareMeters(realEstateArticleNode)
        };
        AddSquareMeterPrice(realEstate);
        AddRealEstateCoordinates(realEstate);
        return realEstate;  
    }

    private string ExtractCleanAddressFromArticleNode(HtmlNode articleNode) {
        var addressNode = ExtractNodeWithXpath(FinnXpathQueries.ADDRESS);
        return ParseAddressString(addressNode.InnerText);
    }

    private string ParseAddressString(string rawAddress) {
        return WebscraperUtilities.ParseAddress(rawAddress);
    }

    private int ExtractCleanRealEstateFinnkode(HtmlNode articleNode) {
        var finnkodeNode = ExtractNodeWithXpath(FinnXpathQueries.FINNKODE);
        return ExtractNumbersFromString(finnkodeNode.InnerText);
    }

    private int ExtractRealEstatePrice(HtmlNode articleNode) {
        var priceNode = ExtractNodeWithXpath(FinnXpathQueries.PRICE);
        return ExtractNumbersFromString(priceNode.InnerText);
    }

    private int ExtractCleanRealEstateSquareMeters(HtmlNode articleNode) {
        var squareMetersNode = ExtractNodeWithXpath(
            FinnXpathQueries.SQUARE_METERS);
        return ExtractNumbersFromString(squareMetersNode.InnerText);
    }

    private void AddSquareMeterPrice(RealEstate realEstate) {
        var squareMeterPrice = realEstate.price / realEstate.squareMeters;
        realEstate.squareMeterPrice = squareMeterPrice;
    }

    private void AddRealEstateCoordinates(RealEstate realEstate) {
        var coordinates = GetAddressCoordinates(realEstate.address);
    }

    private int ExtractNumbersFromString(string stringWithNumbers) {
        var extractedNumbers = WebscraperUtilities
            .ExtractNums(stringWithNumbers);
        return int.Parse(extractedNumbers);
    }

    private HtmlNode ExtractNodeWithXpath(string xpathQuery, HtmlNode node) {
        return node.SelectSingleNode(xpathQuery);
    }


    private string GetArticleUrl(HtmlNode htmlArticle) {
        return htmlArticle.SelectSingleNode(".//a")
            .GetAttributeValue("href", "");
    }
}