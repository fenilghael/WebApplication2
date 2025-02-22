using Microsoft.AspNetCore.Mvc;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class J1Controller : ControllerBase
{
    /// <summary>
    /// Calculates the final game score for Deliv-e-droid.
    /// </summary>
    /// <param name="obstacleCollisions">Number of times the robot collided with obstacles</param>
    /// <param name="packagesDelivered">Number of packages delivered </param>
    /// get  url -X 'POST' 'https://localhost:7018/api/J1/Delivedroid?obstacleCollisions=3&packagesDelivered=2'
    /// result 70
    [HttpPost("Delivedroid")]
    public IActionResult GetFinalScore([FromQuery] int obstacleCollisions, [FromQuery] int packagesDelivered)
    {
        if (obstacleCollisions < 0 || packagesDelivered < 0)
        {
            return BadRequest("Obstacle collisions and packages delivered must be non-negative integers.");
        }

        int totalScore = (packagesDelivered * 50) - (obstacleCollisions * 10);
        if (packagesDelivered > obstacleCollisions)
        {
            totalScore += 500;
        }

        return Ok(new { Score = totalScore });
    }
}




public class J1SushiController : ControllerBase
{
    /// <summary>
    /// Computes the total price of sushi plates based on their quantities.
    /// </summary>
    /// <param name="redCount">Count of red plates selected</param>
    /// <param name="greenCount">Count of green plates selected</param>
    /// <param name="blueCount">Count of blue plates selected</param>
    /// <returns>Total cost of the selected sushi plates</returns>
    /// <example>
    /// GET https://localhost:7018/CalculateBill?redCount=0&greenCount=2&blueCount=4
    /// result 28
    /// </example>
    [HttpGet("CalculateBill")]
    public IActionResult CalculateTotalPrice([FromQuery] int redCount, [FromQuery] int greenCount, [FromQuery] int blueCount)
    {
        int redPrice = 3;
        int greenPrice = 4;
        int bluePrice = 5;

        int finalAmount = (redCount * redPrice) + (greenCount * greenPrice) + (blueCount * bluePrice);

        return Ok(finalAmount);
    }
}


//J2

public class J2Controller : ControllerBase
{
    private readonly Dictionary<string, int> spiceLevels = new Dictionary<string, int>
    {
        { "Poblano", 1500 },
        { "Mirasol", 6000 },
        { "Serrano", 15500 },
        { "Cayenne", 40000 },
        { "Thai", 75000 },
        { "Habanero", 125000 }
    };

    /// <summary>
    /// Computes the total spiciness of a chili dish based on added pepper ingredients.
    /// </summary>
    /// <param name="pepperList">Comma separated list of peppers used</param>
    /// <returns>Total Scoville Heat Units (SHU) of the dish</returns>
    /// <example>
    /// GET https://localhost:7018/ChiliPeppers?pepperList=Poblano%2CCayenne%2CThai%2CPoblano
    /// result 118000
    /// </example>
    [HttpGet("ChiliPeppers")]
    public IActionResult CalculateSpiciness([FromQuery] string pepperList)
    {
        var selectedPeppers = pepperList.Split(',');
        int totalSpiciness = selectedPeppers.Sum(pepper => spiceLevels.ContainsKey(pepper) ? spiceLevels[pepper] : 0);
        return Ok(totalSpiciness);
    }
}

public class SilentAuctionController : ControllerBase
{
    private static readonly Dictionary<string, int> auctionBids = new Dictionary<string, int>();

    /// <summary>
    /// Submits a bid for the silent auction.
    /// </summary>
    /// <param name="bidderName">Bidder's name</param>
    /// <param name="bidAmount">Bid amount</param>
    /// Get curl -X 'POST''https://localhost:7018/SubmitBid?bidderName=fenil&bidAmount=50'
    /// result Bid submitted: Fenil - $50
    [HttpPost("SubmitBid")]
    public IActionResult SubmitBid([FromQuery] string bidderName, [FromQuery] int bidAmount)
    {
        if (!auctionBids.ContainsKey(bidderName))
        {
            auctionBids[bidderName] = bidAmount;
        }
        return Ok($"Bid submitted: {bidderName} - ${bidAmount}");
    }

    /// <summary>
    /// Determines the winner of the silent auction.
    /// </summary>
    /// Get curl -X 'GET''https://localhost:7018/GetWinner'
    /// result Fenil
    [HttpGet("GetWinner")]
    public IActionResult GetWinner()
    {
        if (auctionBids.Count == 0)
        {
            return NotFound("No bids have been placed.");
        }

        var highestBidder = auctionBids.OrderByDescending(b => b.Value).First();
        return Ok(highestBidder.Key);
    }
}


//J3
public class J3CompressionController : ControllerBase
{
    /// <summary>
    /// Performs Run-Length Encoding on input text lines.
    /// </summary>
    /// <param name="textLines">List of strings to be compressed</param>
    /// <returns>Encoded strings using run-length encoding</returns>
    /// <example>
    /// curl -X 'GET' 'https://localhost:7018/CompressText?textLines=4''
    /// result ["1 4"]
    /// </example>
    [HttpGet("CompressText")]
    public IActionResult EncodeTextLines([FromQuery] List<string> textLines)
    {
        if (textLines == null || textLines.Count == 0)
            return BadRequest("At least one text line is required.");

        List<string> compressedResults = new List<string>();

        foreach (string text in textLines)
        {
            StringBuilder compressedText = new StringBuilder();
            int repeatCount = 1;

            for (int index = 1; index <= text.Length; index++)
            {
                if (index == text.Length || text[index] != text[index - 1])
                {
                    compressedText.Append($"{repeatCount} {text[index - 1]} ");
                    repeatCount = 1;
                }
                else
                {
                    repeatCount++;
                }
            }

            compressedResults.Add(compressedText.ToString().Trim());
        }

        return Ok(compressedResults);
    }
}
