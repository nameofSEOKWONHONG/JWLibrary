namespace JWLibrary.Core.NetFramework.DB.MSSQL {

    public interface IDataBinder<T> {

        T DataBind();
    }
}