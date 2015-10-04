<?php
namespace BackendBundle\ClientModels;

class TemperatureGraph
{
    public $device;
    public $readings;
    
    private $em;
    private $start;
    private $end;
    
    public function __construct($em){
        $this->em = $em;
        $this->readings = array();
    }
    
    public function setDevice($device){
        $this->device = $device;
    }
    
    public function fillLast7DaysReadings(){
        $this->start = \date("Y-m-d", strtotime("-7 days"));
        $this->end = \date("Y-m-d");
        
        $this->doFill();
    }
    
    public function fillLastMonthReadings(){
        $this->start = \date("Y-m-d", strtotime("-1 month"));
        $this->end = \date("Y-m-d");
        
        $this->doFill();
    }
    
    private function doFill(){
        $query = $this->em->createQuery('SELECT r
            FROM BackendBundle\Entity\TemperatureDayReading r
            WHERE r.device = :deviceId
            AND r.date BETWEEN :start AND :end'
        );
        
        $query->setParameter('deviceId', $this->device->getId())
        ->setParameter('start', $this->start)
        ->setParameter('end', $this->end);
        $this->readings = $query->getResult();
    }
    
 /**
     * @return the $device
     */
    public function getDevice()
    {
        return $this->device;
    }

 /**
     * @return the $readings
     */
    public function getReadings()
    {
        return $this->readings;
    }

 /**
     * @param multitype: $readings
     */
    public function setReadings($readings)
    {
        $this->readings = $readings;
    }

    
    
}