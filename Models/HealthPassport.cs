namespace PassportMD.Models
{
    public class HealthPassport
    {
        public PersonProfile Profile { get; set; }
        public Vitals? Vitals { get; set; }

        public HealthPassport(PersonProfile profile)
        {
            Profile = profile;
            Vitals = null;
        }

        public void RecordVitals(int systolic, int diastolic, int heartRate)
        {
            Vitals = new Vitals(systolic, diastolic, heartRate);
        }

        public bool HasVitals()
        {
            return Vitals != null;
        }

        public List<string> GetHealthFlags()
        {
            List<string> flags = new List<string>();

            string bmiCat = Profile.GetBmiCategory();
            if (bmiCat == "Underweight")
                flags.Add("BMI is below healthy range");
            else if (bmiCat == "Overweight")
                flags.Add("BMI is above healthy range");
            else if (bmiCat == "Obese")
                flags.Add("BMI indicates obesity - consult a healthcare provider");

            if (HasVitals())
            {
                string bpCat = Vitals.GetBpCategory();
                if (bpCat != "Normal")
                    flags.Add($"Blood pressure is {bpCat.ToLower()}");

                string hrCat = Vitals.GetHeartRateCategory();
                if (hrCat != "Normal")
                    flags.Add($"Heart rate is {hrCat.ToLower()}");
            }
            else
            {
                flags.Add("Vitals not yet recorded - enter vitals for a complete assessment");
            }

            return flags;
        }
    }
}
