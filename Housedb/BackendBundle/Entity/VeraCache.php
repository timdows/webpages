<?php
namespace BackendBundle\Entity;

use Doctrine\ORM\Mapping as ORM;

/**
 * @ORM\Entity
 * @ORM\HasLifecycleCallbacks
 * @ORM\Table(name="tbl_vera_caches")
 */
class VeraCache 
{
    /**
     * @ORM\Column(type="integer")
     * @ORM\Id
     * @ORM\GeneratedValue(strategy="AUTO")
     */
    protected $id;
    
    /**
     * @ORM\Column(type="string", length=255)
     */
    protected $variable;
    
    /**
     * @ORM\Column(type="decimal", scale=2)
     */
    protected $value;
    
    /**
     * @ORM\Column(type="datetime")
     */
    protected $updated;
    
    /**
     * @ORM\PrePersist
     * @ORM\PreUpdate
     */
    public function updatedTimestamps()
    {
        $this->setUpdated(new \DateTime(date('Y-m-d H:i:s')));
    }
    
    public function __construct($variable, $value){
        $this->variable = $variable;
        $this->value = $value;
    }
    
 /**
     * @return the $id
     */
    public function getId()
    {
        return $this->id;
    }

 /**
     * @return the $variable
     */
    public function getVariable()
    {
        return $this->variable;
    }

 /**
     * @return the $value
     */
    public function getValue()
    {
        return $this->value;
    }

 /**
     * @param field_type $id
     */
    public function setId($id)
    {
        $this->id = $id;
    }

 /**
     * @param field_type $variable
     */
    public function setVariable($variable)
    {
        $this->variable = $variable;
    }

 /**
     * @param field_type $value
     */
    public function setValue($value)
    {
        $this->value = $value;
    }
    
 /**
     * @return the $updated
     */
    public function getUpdated()
    {
        return $this->updated;
    }

 /**
     * @param datetime $updated
     */
    public function setUpdated($updated)
    {
        $this->updated = $updated;
    }


    
    
}