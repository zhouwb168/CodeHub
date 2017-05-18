namespace GEIIO.Persistence
{
    public interface IPersistence
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        void Save<T>(T t);

        /// <summary>
        /// 加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Load<T>();

        /// <summary>
        /// 删除
        /// </summary>
        void Delete();
    }
}
