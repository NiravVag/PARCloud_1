import React, { useState, useEffect } from 'react';

// react component used to create charts
import { Line } from 'react-chartjs-2';

// react-bootstrap components
import {
    Badge,
    Button,
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
    ListGroup
} from "react-bootstrap";


const allChartLabels = [
    "1 AM",
    "2 AM",
    "3 AM",
    "4 AM",
    "5 AM",
    "6 AM",
    "7 AM",
    "8 AM",
    "9 AM",
    "10 AM",
    "11 AM",
    "12 PM",
    "1 PM",
    "2 PM",
    "3 AM",
    "4 AM",
    "5 AM",
    "6 AM",
    "7 PM",
    "8 PM",
    "9 PM",
    "10 PM",
    "11 PM",
    "12 AM",
];


const TenantsScaleMeasurementChart2 = (props) => {

    const [isLoading, setIsLoading] = useState(false);
    
    const [chartSeries, setChartSeries] = useState([]);
    
    const [chartData, setChartData] = useState();


    useEffect(() => {
        setIsLoading(true);

        fetch('api/Scale/GetTenantScaleMeasureCounts')
            .then(response => response.json())
            .then(data => {                
                if (data && data.tenantScaleMeasureCounts && data.tenantScaleMeasureCounts.length > 0) {
                    data.tenantScaleMeasureCounts.forEach(ele => {
                        let row = []
                        for (const key in ele) {
                            if (key != "tenantId" && key != "tenantName") {
                                if (ele[key] >= 0) {
                                    row.push(ele[key]);
                                }
                            }
                        }

                        let rowData = {
                            label: ele.tenantName,
                            fill: false,
                            lineTension: 0.5,
                            backgroundColor: 'rgba(3, 112, 186, 1)',
                            borderColor: 'rgba(0,0,0,1)',
                            borderWidth: 2,
                            data: row
                        }

                        chartSeries.push(rowData);

                    });

                    setChartSeries(chartSeries);

                    let labels = [];
                    for (var i = 0; i < chartSeries[0].data.length; i++) {
                        labels.push(allChartLabels[i])
                    }

                    setChartData({
                        labels: labels,
                        datasets: chartSeries,
                    });
                }

                setIsLoading(false);
            });
    }, []);

    
    let chartContent = (
        <Line width={null} height={null}
            data={chartData}
            options={{
                title: {
                    display: true,
                    text: 'Scale Measure Counts Since Midnight',
                    fontSize: 20
                },
                legend: {
                    display: true,
                    position: 'right'
                },
                maintainAspectRatio: false
            }}
        />
    );

    if (!chartData) {
        chartContent = <p className="card-category align-self-center">No Data to Display</p>
    }

    return (
        <Card className="dashboard-card ">
            <Card.Header>
                <Card.Title as="h4">Scale Measure Counts</Card.Title>
                <p className="card-category">Since Midnight</p>
            </Card.Header>
            <Card.Body>
                <div className="d-flex justify-content-center dashboard-chart-container">
                    {chartContent}
                </div>
            </Card.Body>
        </Card>
    );
}

export default TenantsScaleMeasurementChart2