function LoadResult(Data)
{
    
    var Value = JSON.parse(JSON.stringify(Data));
    if (Value.Result == "Success")
    {
        window.location = Value.URL;
    }
    if (Value.Result == "Fail")
    {
       // $("#btnLogin").val("Login");
        $('#btnLogin').removeAttr('disabled');
        $('#btnLoginDB').removeAttr('disabled');
        $("#divMessage").text(Value.Message);
    }
   
}
function Loader()
{
    $("#divMessage").text("");
   
    $('#btnLogin').attr('disabled', 'disabled');
    $('#btnLoginDB').attr('disabled', 'disabled');
}