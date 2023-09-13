/*eslint-disable*/
import React, { Fragment } from "react";

import classnames from "classnames";

import {
    useTable,
    /*useFilters,*/
    useAsyncDebounce,
    useGlobalFilter,
    useSortBy,
    setSortBy,
    useResizeColumns,
    
    usePagination,
    useExpanded
} from "react-table";



// A great library for fuzzy filtering/sorting items
import { matchSorter } from "match-sorter";

// react plugin used to create DropdownMenu for selecting items
import Select from "react-select";

// reactstrap components
import { Container, Row, Col, FormGroup, Input } from "reactstrap";

// react-bootstrap components
import {
    Pagination,
} from "react-bootstrap";


// Define a default UI for filtering
function GlobalFilter({
    preGlobalFilteredRows,
    globalFilter,
    setGlobalFilter
}) {
    const count = preGlobalFilteredRows.length;
    const [value, setValue] = React.useState(globalFilter);
    const onChange = useAsyncDebounce((value) => {
        setGlobalFilter(value || undefined);
    }, 200);

    return (
        
        <span className="form-group has-search">
            <span className="fa fa-search form-control-feedback"></span>
           
            <input className="search-styles form-control"  
                value={value || ""}
                onChange={(e) => {
                    setValue(e.target.value);
                    onChange(e.target.value);
                }}
                placeholder={`Search ${count} records...`}
                style={{
                    fontSize: "1.1rem",
                    border: "0"
                }}
            />
            </span>
        
    );
}

// Let the table remove the filter if the string is empty
//fuzzyTextFilterFn.autoRemove = (val) => !val;

// Our table component
function Table({ columns, data, renderRowSubComponent, header = null}) {
    const [numberOfRows, setNumberOfRows] = React.useState({
        value: 10,
        label: "10 rows",
    });

    const [pageSelect, handlePageSelect] = React.useState({
        value: 0,
        label: "Page 1",
    });

    ////const filterTypes = React.useMemo(
    ////    () => ({
    ////        text: (rows, id, filterValue) => {
    ////            return rows.filter((row) => {
    ////                const rowValue = row.values[id];
    ////                return rowValue !== undefined
    ////                    ? String(rowValue)
    ////                        .toLowerCase()
    ////                        .startsWith(String(filterValue).toLowerCase())
    ////                    : true;
    ////            });
    ////        }
    ////    }),
    ////    []
    ////);    

    ////const instance = useTable(
    ////    {
    ////        columns,
    ////        data
    ////    },
    ////    useFilters, // useFilters!
    ////    useGlobalFilter // useGlobalFilter!
    ////);


    const {
        getTableProps,
        getTableBodyProps,
        footerGroups,
        rows,
        prepareRow,
        state,
        preGlobalFilteredRows,
        setGlobalFilter,
        headerGroups,
        page,        
        visibleColumns,
        nextPage,
        pageOptions,
        pageCount,
        previousPage,
        canPreviousPage,
        canNextPage,
        setPageSize,
        gotoPage,
        state: { expanded }
    }  = useTable(
        {
            columns,
            data,
            // defaultColumn, // Be sure to pass the defaultColumn option
            /*filterTypes,*/
            initialState: {
                globalFilter: "", pageSize: 20, pageIndex: 0,
                hiddenColumns: columns.map(column => {
                    if (column.show === false) return column.accessor || column.id;
                }),
            },

            autoResetPage: false,
            autoResetGlobalFilter: false,
            //autoResetExpanded: false,
            //autoResetGroupBy: false,
            //autoResetSelectedRows: false,
            autoResetSortBy: false,
            autoResetFilters: false,
            //autoResetRowState: false,
            //autoResetFilters: false
        },
        
        //useFilters, // useFilters!
        useGlobalFilter,
        useSortBy,
        useExpanded,
        usePagination,
        //  useResizeColumns,
    );

    // We don't want to render all of the rows for this example, so cap
    // it for this use case
    // const firstPageRows = rows.slice(0, 10);
    let pageSelectData = Array.apply(
        null,
        Array(pageOptions.length)
    ).map(function () { });

    let numberOfRowsData = [5, 10, 20, 25, 50, 100];

    let pagingOptions = pageSelectData.map((prop, key) => {
        if (key === 10) {
            return (<Pagination.Ellipsis key={key} />);
        }
        if (key < 10) {
            return (
                <Pagination.Item key={key} active={pageSelect.value === key}
                    onClick={(value) => {
                        gotoPage(key);
                        handlePageSelect({ value: key, label: "Page " + (key + 1) });
                    }}

                >
                    {key + 1}
                </Pagination.Item>);
        }        
    });

    return (
        <>
            <div className="ReactTable -striped -highlight primary-pagination">
                <div className="pagination-top">
                
                </div>

                <div>
                  
                        
                        <GlobalFilter
                            preGlobalFilteredRows={preGlobalFilteredRows}
                            globalFilter={state.globalFilter}
                            setGlobalFilter={setGlobalFilter}
                        />
                    
                </div>
                <table {...getTableProps()} className="rt-table">


                    <thead className="rt-thead -header">
                        {headerGroups.map((headerGroup) => (
                            <tr {...headerGroup.getHeaderGroupProps()} className="rt-tr">
                                {headerGroup.headers.map((column, key) => (
                                    <th
                                   
                                   {...column.getHeaderProps(column.getSortByToggleProps())}
                                      
                                        className={classnames("text-center rt-th rt-resizable-header", {
                                            "-cursor-pointer": headerGroup.headers.length - 1 !== key,
                                            "-sort-asc": column.isSorted && !column.isSortedDesc,
                                            "-sort-desc": column.isSorted && column.isSortedDesc,
                                        })}
                                        style={{ minWidth: column.minWidth, width: column.width }}
                                    >
                                        <div className="rt-resizable-header-content">
                                            {column.render("Header")}
                                        </div>
                                    
                                    </th>
                                ))}
                            </tr>
                        ))}                       
                    </thead>
                    
                    <tbody {...getTableBodyProps()} className="rt-tbody">
                        {page.map((row, i) => {
                            prepareRow(row);
                            return (
                                <Fragment key={row.getRowProps().key}>
                                    <tr
                                        {...row.getRowProps()}
                                        className={classnames(
                                            "rt-tr",
                                            { " -odd": i % 2 === 0 },
                                            { " -even": i % 2 === 1 }
                                        )}
                                    >
                                        {row.cells.map((cell) => {
                                            return (
                                                <td className={cell.column.className} {...cell.getCellProps({
                                                    style: {
                                                        minWidth: cell.column.minWidth,
                                                        width: cell.column.width,
                                                    
                                                    },
                                                
                                              
                                                })}
                                                >


                                                    {cell.render("Cell")}
                                                </td>
                                            );
                                        })}
                                    </tr>
                                    {row.isExpanded && (
                                        <div>
                                                {renderRowSubComponent(row)}
                                           </div>
                                    )}
                                </Fragment>
                            );
                        })}
                    </tbody>
                </table>
                <div className="pagination-bottom">
                    <div className="-pagination">                       
                        <div className="-center">
                            <Container>
                                <Row className="justify-content-end">
                                    {(pagingOptions && pagingOptions.length > 0) &&
                                        <Pagination className="pagination pagination-no-border">
                                        <Pagination.Item key={-1} disabled={!canPreviousPage} onClick={
                                                () => {
                                                    previousPage()
                                                    handlePageSelect({ value: pageSelect.value - 1, label: "Page " + (pageSelect.value) });
                                                }}>«</Pagination.Item>
                                            {
                                                pagingOptions
                                            }
                                        <Pagination.Item key={100} disabled={!canNextPage} onClick={() => {
                                                nextPage();
                                                handlePageSelect({ value: pageSelect.value + 1, label: "Page " + (pageSelect.value + 1) });
                                            }


                                            }>»</Pagination.Item>
                                        </Pagination>
                                    }
                                    
                                </Row>
                            </Container>
                            
                        </div>                        
                    </div>
                </div>
            </div>

          

        </>
    );
}

// Define a custom filter filter function!
function filterGreaterThan(rows, id, filterValue) {
    return rows.filter((row) => {
        const rowValue = row.values[id];
        return rowValue >= filterValue;
    });
}

// This is an autoRemove method on the filter function that
// when given the new filter value and returns true, the filter
// will be automatically removed. Normally this is just an undefined
// check, but here, we want to remove the filter if it's not a number
filterGreaterThan.autoRemove = (val) => typeof val !== "number";

export default Table;
