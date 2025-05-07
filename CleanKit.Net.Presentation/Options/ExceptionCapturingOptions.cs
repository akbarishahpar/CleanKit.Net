namespace CleanKit.Net.Presentation.Options;

public class ExceptionCapturingOptions
{
    //Control that if exception should be passed to upper middleware or not
    public bool Passthrough { get; set; }
}