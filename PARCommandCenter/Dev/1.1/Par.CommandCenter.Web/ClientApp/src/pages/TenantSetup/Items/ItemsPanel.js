import React, { useEffect, useState } from 'react';

import ReactTable from '../../../components/Common/ReactTable/ReactTable';

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

const ItemsPanel = (props) => {   
    const [isLoading, setIsLoading] = useState(true);
    const [items, setItems] = useState([]);

    let itemsTable = undefined;
    let data = [];

    const url = "api/Item/GetByTenantId/" + props.tenant.id;

    useEffect(() => {
        const fetchData = async (url) => {
            try {
                setIsLoading(true);

                const response = await fetch(url);

                if (response.status === 204) {
                    setItems([]);
                } else {
                    const data = await response.json();
                    setItems(data.items);
                }

                setIsLoading(false);

            } catch (error) {
                setIsLoading(false);
                throw error;
            }
        };

        fetchData(url);
    }, [url, props.onRefresh]);

    if (items && items.length > 0) {
        data = items.map((prop, key) => {
            return {
                id: prop.id,
                location: prop.locationName,
                scaleAddress: prop.scaleAddress,
                itemName: prop.itemName,
                itemNumber: prop.itemNumber,
                itemType: prop.itemType,
                referenceWeight: prop.referenceWeight,
                quantity: prop.quantity,
            };
        });
    }

    itemsTable = (
        <ReactTable
            data={data}
            columns={[
                {
                    Header: "Location",
                    accessor: "location",
                    className: 'rt-first',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },
                {
                    Header: "Scale Address",
                    accessor: "scaleAddress",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },
                {
                    Header: "Item Name",
                    accessor: "itemName",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },
                {
                    Header: "Item Number",
                    accessor: "itemNumber",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 80,
                    width: 100,
                },                
                {
                    Header: "Item Type",
                    accessor: "itemType",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 80,
                    width: 100,
                },                
                {
                    Header: "Reference Weight",
                    accessor: "referenceWeight",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 90,
                    width: 90,
                },
                {
                    Header: "Quantity",
                    accessor: "quantity",
                    className: 'rt-custom',
                    maxWidth: 70,
                    minWidth: 70,
                    width: 70,
                },                                
                //{
                //    Header: "Actions",
                //    accessor: "actions",
                //    className: 'rt-custom',
                //    sortable: false,
                //    filterable: false,
                //    maxWidth: 70,
                //    minWidth: 70,

                //    width: 70,
                //},
            ]}
            /*
                You can choose between primary-pagination, info-pagination, success-pagination, warning-pagination, danger-pagination or none - which will make the pagination buttons gray
            */
            className="-striped -highlight primary-pagination"
        />
    );

    return (
        <>            
            <Card.Body className="table-full-width">
                <Row>
                    <Col className="mx-auto setup-items-table pt-2">
                        {itemsTable}
                    </Col>
                </Row>
            </Card.Body>            
        </>
    );
}

export default ItemsPanel;
