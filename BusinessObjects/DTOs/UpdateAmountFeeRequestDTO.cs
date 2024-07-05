namespace BusinessObjects.DTOs {
    public class UpdateAmountFeeRequestDTO {
        public List<UpdateAmountFeeRequestDTOChild>? Children { get; set; }
    }

    public class UpdateAmountFeeRequestDTOChild {
        public int Id { get; set; }
        public double Amount { get; set; }
    }
}
