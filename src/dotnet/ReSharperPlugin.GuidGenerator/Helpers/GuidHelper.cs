namespace ReSharperPlugin.GuidGenerator.Helpers;

using JetBrains.DocumentModel;

public static class GuidHelper
{
    public static Action<ITextControl> InsertGuid(ICSharpContextActionDataProvider provider, string textToInsert)
    {
        ICSharpTypeDeclaration typeDeclaration = provider.GetSelectedElement<ICSharpTypeDeclaration>();
        if (typeDeclaration is null)
            return null;

        return textControl =>
        {
            int caretOffset = textControl.Caret.Position.Value.ToDocOffset().AsDocumentOffset(textControl.Document).Offset;
            using (WriteLockCookie.Create())
            {
                textControl.Document.InsertText(new DocumentOffset(textControl.Document, caretOffset), textToInsert);
                textControl.Caret.MoveTo(caretOffset + textToInsert.Length, CaretVisualPlacement.DontScrollIfVisible);
            }
        };
    }
}
