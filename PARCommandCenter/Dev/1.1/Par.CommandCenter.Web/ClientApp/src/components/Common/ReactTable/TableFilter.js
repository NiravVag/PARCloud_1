import React from "react";

const TableFilter = ({ setGlobalFilter }) => {   

    return (

        <span className="form-group has-search">
            <span className="fa fa-search form-control-feedback"></span>

            <input className="search-styles form-control"                
                onChange={e => {
                    setGlobalFilter(e.target.value);
                }}
                placeholder={`Search ${10} records...`}
                style={{
                    fontSize: "1.1rem",
                    border: "0"
                }}
            />
        </span>

    );
};

export default TableFilter;
