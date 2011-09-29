
using System.Collections.Generic;

namespace PdfCustomization.Models
{
    public class UserModel
    {
        public UserModel()
        {
            Commitments = new List<QuarterlyCommitment>();
            Prices = new List<Price>();
        }

        public string Name { get; set; }

        public IList<QuarterlyCommitment> Commitments { get; set; }
        public IList<Price> Prices { get; set; }
    }
}