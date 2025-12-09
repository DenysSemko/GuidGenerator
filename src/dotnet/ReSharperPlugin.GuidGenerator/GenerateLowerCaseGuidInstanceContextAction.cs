namespace ReSharperPlugin.GuidGenerator;

[ContextAction(
    GroupType = typeof(CSharpContextActions),
    Name = nameof(GenerateLowerCaseGuidInstanceContextAction),
    Description = nameof(GenerateLowerCaseGuidInstanceContextAction),
    Priority = -10)]
public class GenerateLowerCaseGuidInstanceContextAction(ICSharpContextActionDataProvider provider) : ContextActionBase
{
    public override string Text => Constants.LowerCase.InstanceText;

    public override bool IsAvailable(IUserDataHolder cache)
    {
        var sourceFile = provider.SourceFile;
        Console.WriteLine("hello");
        if (!sourceFile.IsValid())
            return false;

        if (!sourceFile.PrimaryPsiLanguage.Is<CSharpLanguage>())
            return false;

        // Caret must be inside a type declaration
        var typeDeclaration = provider.GetSelectedElement<ICSharpTypeDeclaration>(true, true);
        if (typeDeclaration == null)
            return false;

        // Don’t show inside comments/strings
        var token = provider.GetSelectedElement<ITokenNode>(true, true);
        if (token != null)
        {
            var tt = token.GetTokenType();
            if (tt.IsComment || tt.IsStringLiteral)
                return false;
        }

        return true;
    }

    protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
    {
        Guid generatedGuid = Guid.NewGuid();
        string textToInsert = $"var lowerCaseGuid = new Guid(\"{generatedGuid}\");";

        return GuidHelper.InsertGuid(provider, textToInsert);
    }
}
