// This class was copied as it is from Par.Command
using System;
using System.Collections;
using System.Collections.Generic;

namespace Par.CommandCenter.Domain.Model
{

    /// <summary>
    /// Provides data and additional attributes in the way expected by the REST client.
    /// </summary>
    /// <remarks>
    /// The JSON format of this object looks like the following when there is no error.
    /// <pre>
    /// {
    ///   success: true,
    ///   total: 0,
    ///   data: []
    /// }
    /// </pre>
    /// The an error is returned as follows.
    /// <pre>
    /// {
    ///   success: false,
    ///   message: '...'
    /// }
    /// </pre>
    /// </remarks>
    public class RestResponse
    {
        /// <summary>
        /// True if no error
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The total number of elements in the Data array
        /// </summary>
        public int? Total { get; set; }

        /// <summary>
        /// The data being returned
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Any error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Create a successful response with no data
        /// </summary>
        public RestResponse() : this(null, 0) { }

        /// <summary>
        /// Create a successful response containing data
        /// </summary>
        /// 
        /// <param name="data">The data</param>
        public RestResponse(object data)
        {
            Success = true;

            if (data == null)
            {
                Total = 0;
                Data = null;
            }
            else if (data is ICollection collection)
            {
                Total = collection.Count;
                Data = data;
            }
            else
            {
                Total = 1;
                Data = new List<object>() { data };
            }
        }

        /// <summary>
        /// Create a successful response containing data and a total
        /// </summary>
        /// 
        /// <param name="data">The data</param>
        /// <param name="total">The total value</param>
        public RestResponse(object data, int total)
        {
            Success = true;
            Total = total;

            if (data is ICollection)
                Data = data;

            else if (data != null)
                Data = new List<object>() { data };
        }

        /// <summary>
        /// Create either a successful or unsuccessful response containing data
        /// </summary>
        /// <param name="success">True to create a successful response, otherwise false</param>
        /// <param name="data">The data</param>
        public RestResponse(bool success, object data)
        {
            Success = success;

            if (data is ICollection)
                Data = data;
            else if (data != null)
                Data = new List<object>() { data };

            if (Data != null)
                Total = ((ICollection)Data).Count;
        }

        /// <summary>
        /// Creates an error response containing the specified error
        /// </summary>
        /// 
        /// <param name="error">The error message</param>
        public RestResponse(Exception error)
        {
            Success = false;
            Message = error.Message;
        }
    }
}
