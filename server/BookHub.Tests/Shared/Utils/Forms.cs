namespace BookHub.Tests.Shared.Utils;

using System.Globalization;
using Features.Authors.Shared;

// Valid multipart bodies for the form endpoints, for tests that drive a workflow through the API.
public static class Forms
{
    public static MultipartFormDataContent Book(
        string title,
        Guid? authorId = null,
        params Guid[] genreIds)
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent(title), "Title" },
            { new StringContent("A valid short description"), "ShortDescription" },
            { new StringContent(new string('l', 200)), "LongDescription" },
        };

        if (authorId is not null)
        {
            form.Add(new StringContent(authorId.Value.ToString()), "AuthorId");
        }

        foreach (var genreId in genreIds)
        {
            form.Add(new StringContent(genreId.ToString()), "Genres");
        }

        return form;
    }

    public static MultipartFormDataContent Author(string name)
        => new()
        {
            { new StringContent(name), "Name" },
            { new StringContent(new string('b', 150)), "Biography" },
            { new StringContent(((int)Nationality.Bulgaria).ToString(CultureInfo.InvariantCulture)), "Nationality" },
            { new StringContent(((int)Gender.Other).ToString(CultureInfo.InvariantCulture)), "Gender" },
        };

    public static MultipartFormDataContent Profile(bool isPrivate)
        => new()
        {
            { new StringContent("First"), "FirstName" },
            { new StringContent("Last"), "LastName" },
            { new StringContent(new DateTime(1995, 1, 1).ToString("O", CultureInfo.InvariantCulture)), "DateOfBirth" },
            { new StringContent(isPrivate ? "true" : "false"), "IsPrivate" },
        };
}
