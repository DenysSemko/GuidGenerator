namespace ReSharperPlugin.GuidGenerator;

[ContextAction(
    GroupType = typeof(CSharpContextActions),
    Name = nameof(GenerateGuidInstanceContextAction),
    Description = nameof(GenerateGuidInstanceContextAction),
    Priority = -10)]
public class GenerateGuidInstanceContextAction(ICSharpContextActionDataProvider provider) : ContextActionBase
{
    public override string Text => Constants.GuidInstance.Text;

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
        const string textToInsert = Constants.GuidInstance.GeneratedGuid;

        return GuidHelper.InsertGuid(provider, textToInsert);
    }
}
