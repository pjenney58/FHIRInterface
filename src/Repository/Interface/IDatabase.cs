/*
 MIT License - IDatabase.cs

Copyright (c) 2021 - Present by Sand Drift Software, LLC
All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Data;

namespace HL7Harmonizer.Store.Interface
{
    public interface IDatabase
    {
        void Connect(string address, string dbname, string port, string username, string password);

        int ExecuteNonQuery(string strQuery, List<KeyValuePair<string, object>> parameterList = null);

        bool ExecuteQuery(string queryString, ref DataSet recordSet, List<KeyValuePair<string, object>> parameterList = null, string tableName = null);

        IEnumerable<T> ExecuteQuery<T>(string query);

        T GetById<T>(int id, string query) where T : new();

        T GetById<T>(string id, string query) where T : new();

        T GetById<T>(Guid id, string query) where T : new();

        T GetByName<T>(string name) where T : new();

        IEnumerable<T> GetListOf<T>(string query, List<KeyValuePair<string, object>> parameterList = null) where T : new();

        T PutRecord<T>(T data);

        IEnumerable<T> PutListOf<T>(IEnumerable<T> data);

        /// <summary>
        /// <c> MapClassList </c> Convers a DataSet into IEnumerable -- SQL focused
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="data"> </param>
        /// <returns> IEnumerable <typeparamref name="T" /> </returns>
        IEnumerable<T> MapClassList<T>(DataTable data) where T : new();

        T MapClassRow<T>(DataRow row) where T : new();
    }
}