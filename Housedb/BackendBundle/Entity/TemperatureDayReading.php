<?php
namespace BackendBundle\Entity;

use Doctrine\ORM\Mapping as ORM;

/**
 * @ORM\Entity
 * @ORM\Table(name="tbl_temperature_readings")
 */
class TemperatureDayReading
{
    /**
     * @ORM\Column(type="integer")
     * @ORM\Id
     * @ORM\GeneratedValue(strategy="AUTO")
     */
    protected $id;
    
    /**
     * @ORM\ManyToOne(targetEntity="Device")
     * @ORM\JoinColumn(name="device_id", referencedColumnName="id")
     */
    protected $device;
    
    /**
     * @ORM\Column(type="date")
     */
    protected $date;
    
    /**
     * @ORM\Column(type="integer")
     */
    protected $highest;
    
    /**
     * @ORM\Column(type="integer")
     */
    protected $lowest;
    
    /**
     * @ORM\Column(type="boolean")
     */
    protected $isValid;
    
 /**
     * @return the $id
     */
    public function getId()
    {
        return $this->id;
    }

 /**
     * @return the $device
     */
    public function getDevice()
    {
        return $this->device;
    }

 /**
     * @return the $date
     */
    public function getDate()
    {
        return $this->date;
    }

 /**
     * @return the $highest
     */
    public function getHighest()
    {
        return $this->highest;
    }

 /**
     * @return the $lowest
     */
    public function getLowest()
    {
        return $this->lowest;
    }

 /**
     * @return the $isValid
     */
    public function getIsValid()
    {
        return $this->isValid;
    }

 /**
     * @param field_type $id
     */
    public function setId($id)
    {
        $this->id = $id;
    }

 /**
     * @param field_type $device
     */
    public function setDevice($device)
    {
        $this->device = $device;
    }

 /**
     * @param field_type $date
     */
    public function setDate($date)
    {
        $this->date = $date;
    }

 /**
     * @param field_type $highest
     */
    public function setHighest($highest)
    {
        $this->highest = $highest;
    }

 /**
     * @param field_type $lowest
     */
    public function setLowest($lowest)
    {
        $this->lowest = $lowest;
    }

 /**
     * @param field_type $isValid
     */
    public function setIsValid($isValid)
    {
        $this->isValid = $isValid;
    }

    
    
}