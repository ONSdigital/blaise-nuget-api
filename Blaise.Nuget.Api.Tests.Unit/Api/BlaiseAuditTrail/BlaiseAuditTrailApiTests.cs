using System;

public class BlaiseAuditTrailTest
{
    public BlaiseAuditTrailTest()
    {
        private readonly ConnectionModel _connectionModel;

    public BlaiseAuditTrailTest()
    {
        _connectionModel = new ConnectionModel();
    }

    [Test]
    public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseFileApi_No_Exceptions_Are_Thrown()
    {
        //act && assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.DoesNotThrow(() => new BlaiseFileApi());
    }
}
}