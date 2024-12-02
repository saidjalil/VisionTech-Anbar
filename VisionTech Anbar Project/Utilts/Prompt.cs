public static class Prompt
{
    public static string ShowDialog(string text, string caption)
    {
        Form prompt = new Form
        {
            Width = 400,
            Height = 150,
            Text = caption
        };

        Label label = new Label { Left = 10, Top = 20, Text = text, AutoSize = true };
        TextBox textBox = new TextBox { Left = 10, Top = 50, Width = 360 };
        Button confirmation = new Button
        {
            Text = "Ok",
            Left = 280,
            Width = 80,
            Top = 80,
            DialogResult = DialogResult.OK
        };

        prompt.Controls.Add(label);
        prompt.Controls.Add(textBox);
        prompt.Controls.Add(confirmation);

        prompt.AcceptButton = confirmation;

        return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
    }
}