<?php
namespace BackendBundle\Entity\Heating;

use Doctrine\ORM\Mapping as ORM;

/**
 * @ORM\Entity
 * @ORM\HasLifecycleCallbacks
 * @ORM\Table(name="tbl_heating_values")
 */
class Value 
{
    /**
     * @ORM\Column(type="integer")
     * @ORM\Id
     * @ORM\GeneratedValue(strategy="AUTO")
     */
    protected $id;
    
    /**
     * @ORM\ManyToOne(targetEntity="Meter")
     * @ORM\JoinColumn(name="meter_id", referencedColumnName="id")
     */
    protected $meter;
    
    /**
     * @ORM\Column(type="integer", nullable=true)
     */
    protected $value;
    
    /**
     * @ORM\Column(type="datetime")
     */
    protected $date;
    
 /**
     * @return the $id
     */
    public function getId()
    {
        return $this->id;
    }

 /**
     * @return the $meter
     */
    public function getMeter()
    {
        return $this->meter;
    }

 /**
     * @return the $value
     */
    public function getValue()
    {
        return $this->value;
    }

 /**
     * @return the $date
     */
    public function getDate()
    {
        return $this->date;
    }

 /**
     * @param field_type $id
     */
    public function setId($id)
    {
        $this->id = $id;
    }

 /**
     * @param field_type $meter
     */
    public function setMeter($meter)
    {
        $this->meter = $meter;
    }

 /**
     * @param field_type $value
     */
    public function setValue($value)
    {
        $this->value = $value;
    }

 /**
     * @param field_type $date
     */
    public function setDate($date)
    {
        $this->date = $date;
    }

    
    
}