<?php
namespace BackendBundle\Entity;

use Doctrine\ORM\Mapping as ORM;

/**
 * @ORM\Entity
 * @ORM\Table(name="tbl_power_kwh_readings")
 */
class PowerKwhDayReading
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
    protected $value;
    
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
     * @return the $value
     */
    public function getValue()
    {
        return $this->value;
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
     * @param field_type $value
     */
    public function setValue($value)
    {
        $this->value = $value;
    }

 /**
     * @param field_type $isValid
     */
    public function setIsValid($isValid)
    {
        $this->isValid = $isValid;
    }
}