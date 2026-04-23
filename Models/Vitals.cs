namespace PassportMD.Models
{
    public class Vitals
    {
        public int Systolic { get; set; }
        public int Diastolic { get; set; }
        public int HeartRate { get; set; }

        public Vitals(int systolic, int diastolic, int heartRate)
        {
            Systolic = systolic;
            Diastolic = diastolic;
            HeartRate = heartRate;
        }

        public string GetReading()
        {
            return $"{Systolic}/{Diastolic}";
        }

        public string GetBpCategory()
        {
            if (Systolic < 120 && Diastolic < 80)
                return "Normal";
            else if (Systolic < 130 && Diastolic < 80)
                return "Elevated";
            else if (Systolic < 140 || Diastolic < 90)
                return "High Blood Pressure - Stage 1";
            else
                return "High Blood Pressure - Stage 2";
        }

        public string GetBpTip()
        {
            string category = GetBpCategory();

            if (category == "Normal")
                return "Keep up your healthy lifestyle!";
            else if (category == "Elevated")
                return "Reduce sodium intake and stay active to prevent hypertension.";
            else if (category == "High Blood Pressure - Stage 1")
                return "Lifestyle changes recommended. Consider consulting a doctor.";
            else
                return "Please consult a healthcare provider as soon as possible.";
        }

        public string GetHeartRateCategory()
        {
            if (HeartRate < 60)
                return "Low (Bradycardia)";
            else if (HeartRate <= 100)
                return "Normal";
            else
                return "High (Tachycardia)";
        }

        public string GetHeartRateTip()
        {
            string category = GetHeartRateCategory();

            if (category == "Low (Bradycardia)")
                return "A low heart rate can be normal for athletes, but consult a doctor if you feel dizzy.";
            else if (category == "Normal")
                return "Your resting heart rate is in a healthy range. Keep it up!";
            else
                return "A high resting heart rate may indicate stress or health issues. Consider consulting a doctor.";
        }
    }
}
