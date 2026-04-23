namespace PassportMD.Models
{
    public class PersonProfile
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double Height { get; set; }  // in inches
        public double Weight { get; set; }  // in pounds

        public PersonProfile(string name, int age, double height, double weight)
        {
            Name = name;
            Age = age;
            Height = height;
            Weight = weight;
        }

        public double CalculateBmi()
        {
            return (Weight / (Height * Height)) * 703;
        }

        public string GetBmiCategory()
        {
            double bmi = CalculateBmi();

            if (bmi < 18.5)
                return "Underweight";
            else if (bmi < 25)
                return "Healthy";
            else if (bmi < 30)
                return "Overweight";
            else
                return "Obese";
        }

        public string GetBmiTip()
        {
            string category = GetBmiCategory();

            if (category == "Underweight")
                return "Consider a nutrient-rich diet to reach a healthy weight.";
            else if (category == "Healthy")
                return "Great job! Maintain your balanced diet and exercise routine.";
            else if (category == "Overweight")
                return "Consider increasing physical activity and watching portion sizes.";
            else
                return "Consult a healthcare provider for a personalized wellness plan.";
        }
    }
}
