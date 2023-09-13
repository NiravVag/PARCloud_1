import React, { useEffect, useState, useCallback } from 'react';

import classnames from "classnames";

import { useTable } from 'react-table'

import {
    Badge,
    Button,
    ButtonGroup,
    Card,
    Form,
    InputGroup,
    Navbar,
    Nav,
    Table,
    Container,
    Row,
    Col,
    OverlayTrigger,
    Tooltip,
    ListGroup,
    Modal,
    FormControl,
    Spinner
} from "react-bootstrap";


import ReactTable from './../../components/Common/ReactTable/ReactTable';


function SubTable({ columns, data }) {
    // Use the state and functions returned from useTable to build your UI
    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        rows,
        prepareRow,
    } = useTable({
        columns,
        data,
    })

    // Render the UI for your table
    return (
        <div className="ReactTable w-75 p-3">
        {/*<table {...getTableProps()} className="rt-table">*/}
        {/*    <thead className="rt-thead -header">*/}
        {/*            {headerGroups.map(headerGroup => (*/}
        {/*                <tr {...headerGroup.getHeaderGroupProps()} className="rt-tr">*/}
        {/*                    {headerGroup.headers.map(column => (*/}
        {/*                        <th {...column.getHeaderProps()} className="text-center rt-th rt-resizable-header -cursor-pointer">{column.render('Header')}</th>*/}
        {/*                ))}*/}
        {/*            </tr>*/}
        {/*        ))}*/}
        {/*    </thead>*/}
        {/*    <tbody {...getTableBodyProps()} className="rt-tbody">*/}
        {/*        {rows.map((row, i) => {*/}
        {/*            prepareRow(row)*/}
        {/*            return (*/}
        {/*                <tr {...row.getRowProps()} className="rt-tr" >*/}
        {/*                    {row.cells.map(cell => {*/}
        {/*                        return <td {...cell.getCellProps()}>{cell.render('Cell')}</td>*/}
        {/*                    })}*/}
        {/*                </tr>*/}
        {/*            )*/}
        {/*        })}*/}
        {/*    </tbody>*/}
            {/*    </table>*/}
            <em>
                The tenant items that are associated with product.
            </em>
            <br/>
            <table {...getTableProps()} className="rt-table">


                <thead className="rt-thead -header">
                    {headerGroups.map((headerGroup) => (
                        <tr {...headerGroup.getHeaderGroupProps()} className="rt-tr">
                            {headerGroup.headers.map((column, key) => (
                                <th

                                    

                                    className="text-center rt-th rt-resizable-header "
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
                    {rows.map((row, i) => {
                        prepareRow(row);
                        return (
                            
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


                                            })
                                            }>


                                                {cell.render("Cell")}
                                            </td>
                                        );
                                    })}
                                </tr>
                                
                            
                        );
                    })}
                </tbody>
            </table>
        </div>
    )
}


const GlobalProductsPanel = (props) => {
    const [isLoading, setIsLoading] = useState(true);
    const [products, setProducts] = useState([]);
    const [errorMessage, setErrorMessage] = useState();

    let tableContent = undefined;
    let data = [];

    const url = "api/Product/GetAll?pageNumber=1&pageSize=1000";

    useEffect(() => {
        const fetchData = async (url) => {
            try {
                setIsLoading(true);

                const response = await fetch(url);

                if (response.status === 204) {
                    setProducts([]);
                } else {
                    const data = await response.json();
                    setProducts(data.products);
                }

                setIsLoading(false);

            } catch (error) {
                setIsLoading(false);
                throw error;
            }
        };

        fetchData(url);
    }, [url, props.onRefresh]);


    const renderRowSubComponent = useCallback(
        (row) => {
            if (row && row.original && row.original.items && row.original.items.length > 0) {
                return (
                                     
                        <SubTable
                        data={row.original.items}
                            ref={el => {
                                if (el) {
                                    console.log(row.index + ' created');
                                } else {
                                    console.log(row.index + ' destroyed');
                                }
                            }}
                            columns={[                                
                                {
                                    Header: "Id",
                                    accessor: "id",
                                    className: 'rt-first',
                                    maxWidth: 100,
                                    minWidth: 30,
                                    with: 50
                                },
                                {
                                    Header: "Tenant Name",
                                    accessor: "tenant.name",
                                    className: 'rt-custom',
                                    maxWidth: 320,
                                    minWidth: 200,
                                },
                                {
                                    Header: "Item Number",
                                    accessor: "number",
                                    className: 'rt-custom',
                                    maxWidth: 120,
                                    minWidth: 50,
                                },
                                {
                                    Header: "Item Name",
                                    accessor: "name",
                                    className: 'rt-custom',
                                    maxWidth: 320,
                                    minWidth: 200,
                                },
                                {
                                    Header: "Vendor Number",
                                    accessor: "vendorNumber",
                                    className: 'rt-custom',
                                    maxWidth: 200,
                                    minWidth: 50,
                                },
                               ]}
                            showPagination={false}
                             />                    
                )
            }
            else {
                return (

                    <div style={{ padding: "20px" }}>
                        <em>
                            No items.
                        </em>
                    </div>
                )
            }
        },
        []
    );
    

    if (products && products.length > 0) {
        data = products.map((prop, key) => {
            return {
                id: prop.id,
                productIdentifier: prop.productIdentifier,                
                shortDescription: prop.shortDescription,
                longDescription: prop.longDescription,
                brandName: prop.brandName,
                items: prop.items,
                manufacturerNumber: prop.manufacturerNumber,
                manufacturerName: (prop.manufacturer) ? prop.manufacturer.name : "",
                categoryName: (prop.category) ? prop.category.name : "",
            };
        });
    }

    if (isLoading) {
        tableContent =
            <Container className="table-spinner">
           
                    <Spinner animation="border" variant="primary" />
           
            </Container>
    } else {

       

        tableContent = (
            <ReactTable
                data={data}
                columns={[
                    {
                        // Build our expander column
                        id: "expander", // Make sure it has an ID
                        minWidth: 20,
                        width: 20,
                        maxWidth: 20,
                        classNames: "rt-first",
                        //Header: ({ getToggleAllRowsExpandedProps, isAllRowsExpanded }) => (
                        //    <div {...getToggleAllRowsExpandedProps()} className={classNames("rt-expander", isAllRowsExpanded && "-open")}>                                
                        //    </div>
                        //),
                        Cell: ({ row }) => {
                            if (row && row.original && row.original.items && row.original.items.length > 0) {
                                return (
                                    // Use Cell to render an expander for each row.
                                    // We can use the getToggleRowExpandedProps prop-getter
                                    // to build the expander.
                                    <div {...row.getToggleRowExpandedProps()} className={classnames("rt-expander", row.isExpanded && "-open")}>
                                    </div>
                                );
                            }
                            return null;
                        }
                    },
                    {
                        Header: "Id",
                        accessor: "id",
                        className: 'rt-custom',
                        maxWidth: 30,
                        minWidth: 30,
                        width: 30,
                    },
                    {
                        Header: "Product Identifier",
                        accessor: "productIdentifier",
                        className: 'rt-custom',
                        maxWidth: 200,
                        minWidth: 50,                      
                    },
                    {
                        Header: "Short Description",
                        accessor: "shortDescription",
                        className: 'rt-custom',
                        maxWidth: 500,
                        minWidth: 320,
                    },
                    {
                        Header: "Long Description",
                        accessor: "longDescription",
                        className: 'rt-custom',
                        maxWidth: 450,
                        minWidth: 320,
                    },
                    {
                        Header: "Brand Name",
                        accessor: "brandName",
                        className: 'rt-custom',
                        maxWidth: 320,
                        minWidth: 100,
                    },
                    {
                        Header: "Manufacturer Number",
                        accessor: "manufacturerNumber",
                        className: 'rt-custom',
                        maxWidth: 320,
                        minWidth: 100,
                    },
                    {
                        Header: "Manufacturer Name",
                        accessor: "manufacturerName",
                        className: 'rt-custom',
                        maxWidth: 320,
                        minWidth: 100,
                    },

                    {
                        Header: "Category Name",
                        accessor: "categoryName",
                        className: 'rt-custom',
                        maxWidth: 320,
                        minWidth: 100,
                    },
                ]}
                /*
                    You can choose between primary-pagination, info-pagination, success-pagination, warning-pagination, danger-pagination or none - which will make the pagination buttons gray
                */
                className="-striped -highlight primary-pagination"

                renderRowSubComponent={renderRowSubComponent }
            />
        );
    }

    return (
        <>
            <Card.Body className="table-full-width">
                <Row>
                    <Col className="mx-auto global-products-table pt-2">
                        {tableContent}
                    </Col>
                </Row>
            </Card.Body>
        </>
    );
}

export default GlobalProductsPanel;
